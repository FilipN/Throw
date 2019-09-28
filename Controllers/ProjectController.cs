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

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private IMemoryCache cache;
        private DataContext repo;

        public ProjectsController(DataContext repository, IMemoryCache memoryCache)
        {
            cache = memoryCache;
            repo = repository;
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
            var rng = repo.NewProject();
            JObject result = new JObject() { { "guid", rng } };
            return result;
        }

        public string getUserRole(string user, string projectGuid)
        {
            return "rw";
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
            string username = "filip";
            //umesto ovoga ce biti uzimanje iz memorije ili baze
            string projectGuid = project["guid"].ToString();
            string userRole = getUserRole(username, projectGuid);
            
            if (userRole == "rw") { 
            
                //odradi lock
            }
            else if (userRole == "r")
            {
                //nema prava
            }

            return new JObject();
        }

        [HttpPost("run")]
        public JObject RunProject([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            //umesto ovoga ce biti uzimanje iz memorije ili baze
            string projectCode = project["code"].ToString();
            string projectGuid = project["guid"].ToString();
            bool projectBlocked = getProjectLock(projectGuid);
            string userRole = getUserRole(username,projectGuid);

            string dirPath = Directory.GetCurrentDirectory();
            string path = dirPath+"\\ActiveProjectSnapshots\\" +username + "_" + projectGuid + ".py";
            string runResult = addFileAndRun(path, projectCode);

            if (userRole == "rw") { 
                //propagira se svima
            }
            else if (userRole == "r")
            {

            }

            return new JObject() { { "runResult", runResult } };
        }

        [HttpPost("open")]
        public JObject OpenProject([FromBody]JObject project)
        {
            string projectGuid = project["guid"].ToString();

            string code = "";
            Code currCode;
            if (!cache.TryGetValue<Code>(projectGuid, out currCode)) { 
                string jproject = repo.GetProjectByGUID(projectGuid);
                JObject projectO = JObject.Parse(jproject);
                currCode = new Code(projectO["Content"].ToString());
            }
            code = currCode.getCode();
            cache.Set<Code>(projectGuid, currCode);

            /*bool projectBlocked = getProjectLock(projectGuid);
            string userRole = getUserRole(username, projectGuid);

            if (userRole == "rw")
            {
                //propagira se svima
            }
            else if (userRole == "r")
            {

            }

            string code = "print('Hello world')"; */


            // return new JObject() { { "code",code }, {"role",userRole } };

            return new JObject() { { "code", code } };
        }

        private bool getProjectLock(string projectGuid)
        {
            return true;
        }


        //pozivace se stalno na key press
        [HttpPost("change")]
        public JObject Change([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();
            int lineNumber = Int32.Parse(project["linenumber"].ToString());
            string ucode = project["code"].ToString();
            string userRole = getUserRole(username, projectGuid);



            //Code projectCode = HttpContext.Session.GetComplexData<Code>(projectGuid);
            Code currCode;
            if(cache.TryGetValue<Code>(projectGuid, out currCode))
            {
                string[] userCode = ucode.Split('\n');

                int minLines = 0;
                if (userCode.Length > currCode.Lines.Count)
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
                    }
                }

                //znaci da imamo obrisanih linija
                if (userCode.Length < currCode.Lines.Count)
                {
                    //dodajemo svaku dodatnu liniju
                    for (int i = minLines; i < currCode.Lines.Count; i++)
                    {
                        currCode.deleteLine(i);
                    }
                }


                //cache.Set<Code>(projectGuid, currCode);
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
    }
}
