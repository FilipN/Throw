using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {

        [HttpGet("{id}")]
        public string ProjectById(string id)
        {
            //proverava prava, u zavisnosti od toga javlja i klijentu koja su prava zbog drugacijeg crtanja
            //vraca aktivni snapshot projekta i 
            var rng = new Random();
            return id;
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

        [HttpPost("")]
        public string New(JObject project)
        {
            //projekat sadrzi ime, mejlove kolaboratora i njihova prava
            //metod vraca link ka projektu
            var rng = new Random();
            return "";
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
        public JObject Lock(JObject project)
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
            string username = "filip";
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
            }else if (userRole == "r")
            {

            }

            return new JObject() { { "runResult", runResult } };
        }

        private bool getProjectLock(string projectGuid)
        {
            return true;
        }
    }
}
