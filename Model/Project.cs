using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Throw.Model
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int OwnerID { get; set; }

        public IEnumerable<AppUser> MembersID { get; set; }

    }

    public class ProjectSnapshot
    {
        public int ProjectId { get; set; }
        public string Content { get; set; }
    }
}
