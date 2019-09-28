using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Throw.Model
{
    public class Project
    {
        public int ProjectId { get; set; }
        
        public Guid ProjectGUID { get; set; }

        public string Title { get; set; }
        public string Content { get; set; } = "print('Hello world!')";

        public User Owner { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }

    }

    public class ProjectUser
    {
        public int ProjectId { get; set; }

        public Project Project { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }

    public class ProjectSnapshot
    {
        public int ProjectSnapshotId { get; set; }

        public Project Project { get; set; }
        public string Content { get; set; }

        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }
    }
}
