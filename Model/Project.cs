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
        public string Content { get; set; }

        public int Owner { get; set; }

        public IEnumerable<User> Members { get; set; }

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
