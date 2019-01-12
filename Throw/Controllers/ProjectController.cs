using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Throw.Models;

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        ProjectDataAccessLayer objproject = new ProjectDataAccessLayer();
        ErrorLogDataAccessLayer error = new ErrorLogDataAccessLayer();

        [HttpPost("Create")]
        public Project Create([FromBody] dynamic input)
        {
            try
            {
                JObject jInput = input as JObject;
                Project project = new Project { Name = input["name"] };
                if(objproject.AddProject(project)!=null)
                {
                    return project;
                }
                return null;   
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        /*
        [HttpGet("Index")]

        public IEnumerable<Project> Index()
        {
            return objproject.GetProjets();
        }
  
        [HttpGet("Details/{id}")]
        public Project Details(int id)
        {
            return objproject.GetProjectData(id);
        }

        [HttpPut("Edit")]
        public int Edit(Project project)
        {
            return objproject.UpdateProject(project);
        }

        [HttpDelete("Delete/{id}")]
        public int Delete(int id)
        {
            return objproject.DeleteProject(id);
        }
        */

    }
}
