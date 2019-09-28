using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Throw.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<ProjectSnapshot> ProjectSnapshots { get; set; }

        public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>()
                .HasKey(bc => new { bc.ProjectId, bc.UserId });

           
        }

        public bool SaveUser(User user)
        {

            User userDB = Users.FirstOrDefault(p => p.Email == user.Email);

            if (userDB == null)
            {
                Users.Add(user);
                SaveChanges();
                return true;
            }
            else
                return false;

        }

        public Guid NewProject()
        {
            Project project = new Project { ProjectGUID = Guid.NewGuid() };
            Projects.Add(project);
            SaveChanges();

            return project.ProjectGUID;

        }
        //Newtonsoft.Json.JsonConvert.SerializeObject(new {foo = "bar"})

        public string GetProjectByGUID(string guid)
        {
            Project project = Projects.FirstOrDefault(p => p.ProjectGUID.ToString() == guid);
            if(project!=null)
            {
                var jproject = JsonConvert.SerializeObject(project);
                return jproject;
            }

            return "";

        }

        public string GetProjectsForUser(string email)
        {
            User user = Users.FirstOrDefault(u => u.Email == email);

            var projsUsers = ProjectUsers.Where(u => u.UserId == user.UserId).ToList();
            List < Project > p= new List<Project>();
            foreach (var el in projsUsers)
            {
                p.Append(el.Project);
            }
            return JsonConvert.SerializeObject(p);

        }
    }
}
