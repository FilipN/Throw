using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Throw.Model;
using Throw.Common;
using Throw.SessionData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.SignalR;
using Throw.Hubs;
using System.Threading;

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private IMemoryCache cache;
        private DataContext repo;
        IHubContext<ProjectHub> hub;

        public ProjectsController(DataContext repository, IMemoryCache memoryCache, IHubContext<ProjectHub> hubcontext)
        {
            cache = memoryCache;
            repo = repository;
            hub = hubcontext;
        }

        private string runPython(string cmd, string args)
        {
            string dirPath = Directory.GetCurrentDirectory();
            string path = dirPath + "\\Python\\python.exe";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = path;
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                string result = "", error = "";
                using (StreamReader reader = process.StandardError)
                {
                    error = reader.ReadToEnd();
                }

                if (!String.IsNullOrEmpty(error)) {
                    string[] errsp = error.Split(',');
                    errsp[0] = "";
                    return string.Join("", errsp);
                }

                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                    return result;
                }
            }
        }

        [HttpPost("new")]
        public JObject New([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            //projekat sadrzi ime, mejlove kolaboratora i njihova prava
            //metod vraca link ka projektu
            var rng = repo.NewProject(username);
            JObject result = new JObject() { { "guid", rng } };
            return result;
        }

        public string addFileAndRun(string path,string content)
        {
            //cuvanje fajla i pokretanje konzole python nad fajlom
            //uzimanje rezultata i vracanje

            System.IO.File.WriteAllText(path, content);
            return runPython(path, "");
        }


        [HttpPost("lock")]
        public JObject Lock([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();

            if (!(repo.GetProjectOwnerByGUID(projectGuid) == username))
                return new JObject();

            string jproject = repo.GetProjectByGUID(projectGuid);

            Code currCode;
            if (!cache.TryGetValue<Code>(projectGuid, out currCode))
            {
                JObject projectO = JObject.Parse(jproject);
                currCode = new Code(projectO["Content"].ToString());
            }

            currCode.Locked = Boolean.Parse(project["lock"].ToString());
            cache.Set<Code>(projectGuid, currCode);

            return new JObject();
        }

        [HttpPost("run")]
        public JObject RunProject([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            //umesto ovoga ce biti uzimanje iz memorije ili baze
            string projectCode = project["code"].ToString();
            string projectGuid = project["guid"].ToString();

            string dirPath = Directory.GetCurrentDirectory();
            string path = dirPath+"\\ActiveProjectSnapshots\\" +username + "_" + projectGuid + ".py";
            string runResult = addFileAndRun(path, projectCode);
            JObject runResultO = new JObject() { { "runResult", runResult } };


            if (repo.GetProjectOwnerByGUID(projectGuid) == username)
                hub.Clients.Groups(projectGuid).SendAsync("outputchange", runResultO);

            return new JObject() { { "runResult", runResult } };
        }

        [HttpPost("open")]
        public JObject OpenProject([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();
            repo.AddMemberToProject(username, projectGuid);
            string jproject = repo.GetProjectByGUID(projectGuid);


            string code = "";
            Code currCode;
            if (!cache.TryGetValue<Code>(projectGuid, out currCode)) {               
                JObject projectO = JObject.Parse(jproject);
                currCode = new Code(projectO["Content"].ToString());
            }
            currCode.addUser(username);
            code = currCode.getCode();
            cache.Set<Code>(projectGuid, currCode);

            JObject result = new JObject();
            if(repo.GetProjectOwnerByGUID(projectGuid)==username)
                result.Add("role", "owner");
            else
                result.Add("role", "member");

            result.Add("project", jproject);
            result.Add("code", code);
            result.Add("users", new JArray( currCode.Users));

            JObject others = new JObject();
            others.Add("users", new JArray(currCode.Users));
            hub.Clients.Groups(projectGuid).SendAsync("usersrefresh", others);

            return result;
        }


        [HttpPost("leave")]
        public JObject leaveProject([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();

            string jproject = repo.GetProjectByGUID(projectGuid);


            Code currCode;
            if (!cache.TryGetValue<Code>(projectGuid, out currCode))
            {
                JObject projectO = JObject.Parse(jproject);
                currCode = new Code(projectO["Content"].ToString());
            }
            currCode.Users.Remove(username);
            cache.Set<Code>(projectGuid, currCode);

            JObject others = new JObject();
            others.Add("users", new JArray(currCode.Users));
            hub.Clients.Groups(projectGuid).SendAsync("usersrefresh", others);

            return new JObject();
        }

        //pozivace se stalno na key press
        [HttpPost("change")]
        public JObject Change([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();
            string ucode = project["code"].ToString();
            bool codeChangeExists = false;


            //Code projectCode = HttpContext.Session.GetComplexData<Code>(projectGuid);
            Code currCode;
            if(cache.TryGetValue<Code>(projectGuid, out currCode))
            {
                string[] userCode = ucode.Split('\n');

                int minLines = 0;
                if (userCode.Length < currCode.Lines.Count)
                    minLines = userCode.Length;
                else
                    minLines = currCode.Lines.Count;

                for (int i = 0; i < minLines; i++)
                {
                    //ako su razlicite
                    if (userCode[i] != currCode.Lines[i].Content)
                    {
                        //proveriti pre promene da nije neki drugi user menjao ovu liniju pre n sekundi
                        currCode.Lines[i].Content = userCode[i];
                        currCode.Lines[i].LastModified = DateTime.Now;
                        codeChangeExists = true;
                        //azurirati usera
                    }
                }

                //znaci da imamo dodatnih linija
                if(userCode.Length > currCode.Lines.Count)
                {
                    //dodajemo svaku dodatnu liniju
                    for(int i =minLines; i< userCode.Length; i++)
                    {
                        currCode.addLine(userCode[i]);
                        codeChangeExists = true;
                    }
                }

                //znaci da imamo obrisanih linija
                if (userCode.Length < currCode.Lines.Count)
                {
                    //dodajemo svaku dodatnu liniju
                    for (int i = minLines; i < currCode.Lines.Count; i++)
                    {
                        currCode.deleteLine(i);
                        codeChangeExists = true;
                    }
                }
            }

            if (codeChangeExists)
            {
                JObject runResultO = new JObject() { { "newCode", currCode.getCode() } };

                string role;
                if (repo.GetProjectOwnerByGUID(projectGuid) == username)
                    role = "owner";
                else
                    role = "member";

                if ((role == "member" && !currCode.Locked)||role=="owner")
                {
                    //propagira se svima
                    hub.Clients.Groups(projectGuid).SendAsync("codechange", runResultO);
                }
            }

            cache.Set<Code>(projectGuid, currCode);
            return new JObject();
        }

        [HttpPost("projectsforuser")]
        public JObject ProjectsForUser([FromBody]JObject userIn)
        {
            string email = userIn["email"].ToString();
            string json = repo.GetProjectsForUser(email);
            JObject result = new JObject() { { "result", json } };
            return result;
        }

        [HttpPost("save")]
        public JObject SaveProject([FromBody]JObject projectIn)
        {
            string username = projectIn["identity"].ToString();
            string projectGuid = projectIn["guid"].ToString();
            string ucode = projectIn["code"].ToString();

            repo.SaveProjectToDatabase(username, projectGuid, ucode);
            JObject result = new JObject() { { "result", "" } };
            return result;
        }
    }
}
