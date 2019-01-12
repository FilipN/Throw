using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Throw.Models
{
    public class ProjectDataAccessLayer
    {

        ThrowSQLContext db = new ThrowSQLContext();
        ErrorLogDataAccessLayer error = new ErrorLogDataAccessLayer();

        public IEnumerable<Project> GetProjets()
        {
            try
            {
                return db.Project.ToList();
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        public int? AddProject(Project project)
        {
            try
            {
                db.Project.Add(project);
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        public int? UpdateProject(Project project)
        {
            try
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        //Get the details of a particular project    
        public bool? CheckIfProjectExists(Project reqProject)
        {
            try
            {
                if (db.Project.Where(u => u.Name == reqProject.Name && u.Link == reqProject.Link).Any())
                    return true;
                return false;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }


        //Get the details of a particular project    
        public Project GetProjectData(int id)
        {
            try
            {
                Project project = db.Project.Find(id);
                return project;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        //To Delete the record of a particular project    
        public int? DeleteProject(int id)
        {
            try
            {
                Project project = db.Project.Find(id);
                db.Project.Remove(project);
                db.SaveChanges();
                return 1;
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }
    }
}
