using System;
using System.Collections.Generic;
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
            return "5";
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
        public JObject RunProject(JObject project)
        {
            string username = "filip";
            //umesto ovoga ce biti uzimanje iz memorije ili baze
            string projectCode = project["code"].ToString();
            string projectGuid = project["guid"].ToString();
            bool projectBlocked = getProjectLock(projectGuid);
            string userRole = getUserRole(username,projectGuid);

            string path = username + "_" + projectGuid + ".py";
            string runResult = addFileAndRun(path, projectCode);
            if (userRole == "rw") { 
                //
            }else if (userRole == "r")
            {

            }

            System.IO.File.WriteAllText("ActiveProjectSnapshots\\"+projectGuid.ToString()+".py",projectCode);
            return new JObject();
        }

        private bool getProjectLock(string projectGuid)
        {
            return true;
        }
    }
}
