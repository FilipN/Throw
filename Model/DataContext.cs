﻿using Microsoft.EntityFrameworkCore;
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

        //public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>()
                .HasKey(bc => new { bc.ProjectId, bc.UserId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(x => x.Project)
                .WithMany(x => x.ProjectUser)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.ProjectUser)
                .HasForeignKey(x => x.UserId);


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

        public Guid NewProject(string username)
        {
            User userDB = Users.FirstOrDefault(p => p.Email == username);
            Project project = new Project { ProjectGUID = Guid.NewGuid() , Owner=userDB};
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
                var jproject = JsonConvert.SerializeObject(project, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return jproject;
            }

            return "";

        }


        public string GetProjectOwnerByGUID(string guid)
        {
            Project project = Projects.Include(p=>p.Owner).FirstOrDefault(p => p.ProjectGUID.ToString() == guid);
            if (project != null)
            {
                return project.Owner.Email;
            }

            return "";

        }

        public void AddMemberToProject(string username, string guid)
        {
            User userDB = Users.FirstOrDefault(p => p.Email == username);
            Project project = Projects.FirstOrDefault(p => p.ProjectGUID.ToString() == guid);

            var data = Projects.Include(p => p.ProjectUser)
                .Any(p => p.ProjectGUID.ToString() == guid && p.ProjectUser.Count > 0 && p.ProjectUser.Any(pu => pu.UserId == userDB.UserId));
            
            //dodavanje usera
            if(data==false)
            {
                ProjectUser pu = new ProjectUser
                {
                    User = userDB,
                    UserId = userDB.UserId,
                    Project = project,
                    ProjectId = project.ProjectId
                };

                Projects.Include(p => p.ProjectUser)
                    .FirstOrDefault(p => p.ProjectGUID.ToString() == guid).ProjectUser.Add(pu);
                SaveChanges();
            }  
            

        }

        public string GetProjectsForUser(string email)
        {
            User user = Users.FirstOrDefault(u => u.Email == email);
            var data = Users
            .Include(u => u.ProjectUser).ThenInclude(project => project.Project)
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault();

            string  json= JsonConvert.SerializeObject(data.ProjectUser.Select(pu=>pu.Project).ToList(), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }
    }
}
