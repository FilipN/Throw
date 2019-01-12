using System;
using System.Collections.Generic;

namespace Throw.Models
{
    public partial class Project
    {
        public Project()
        {
            ProjectSnapshot = new HashSet<ProjectSnapshot>();
            UserProject = new HashSet<UserProject>();
        }

        public int Idproject { get; set; }
        public string Name { get; set; }
        public Guid Link { get; set; }

        public ICollection<ProjectSnapshot> ProjectSnapshot { get; set; }
        public ICollection<UserProject> UserProject { get; set; }
    }
}
