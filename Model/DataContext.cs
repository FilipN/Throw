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

        public string GetSnapshotsByGUID(string guid)
        {

            var prsnap = ProjectSnapshots.Include(ps => ps.Project).Where(ps => ps.Project.ProjectGUID.ToString() == guid).ToList();
            string json = JsonConvert.SerializeObject(prsnap, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;

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

        public void SaveProjectToDatabase(string username, string guid, string code)
        {
            User userDB = Users.FirstOrDefault(p => p.Email == username);
            Project project = Projects.FirstOrDefault(p => p.ProjectGUID.ToString() == guid);

            if( Projects.Include(p=>p.Owner).FirstOrDefault(p=>p.ProjectGUID.ToString()==guid).Owner.UserId==userDB.UserId)
            {
                project.Content = code;
                SaveChanges();
            }
            
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

            var projects = Projects
            .Include(p => p.Owner)
            .Include(u => u.ProjectUser).ThenInclude(project => project.Project)
            .Where(p => p.Owner.UserId == user.UserId || p.ProjectUser.Any(pu => pu.UserId == user.UserId));

            List<ProjectView> pwlist = new List<ProjectView>();

            foreach (var pr in projects)
            {
                ProjectView pw = new ProjectView();
                pw.ProjectGUID = pr.ProjectGUID;
                pw.Content = pr.Content;
                pw.Title = pr.Title;

                pw.Users = new List<string>();
                pw.Users.Add(pr.Owner.Email);
                foreach (var projectuser in pr.ProjectUser)
                {
                    string e = Users.FirstOrDefault(u => u.UserId == projectuser.UserId).Email;
                    if (!pw.Users.Contains(e))
                        pw.Users.Add(e);
                }

                pwlist.Add(pw);

            }

            string json = JsonConvert.SerializeObject(pwlist);

            return json;
        }

        public class ProjectView
        {
            public Guid ProjectGUID { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }

            public List<string> Users { get; set; }


        }
    }
}
