using Microsoft.EntityFrameworkCore;
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

        public bool NewProject(Project project)
        {

            Projects.Add(project);
            SaveChanges();
            return true;

        }
    }
}
