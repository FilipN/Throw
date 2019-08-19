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
        public string NewProject(JObject project)
        {
            //projekat sadrzi ime, mejlove kolaboratora i njihova prava
            //metod vraca link ka projektu
            var rng = new Random();
            return "";
        }
    }
}
