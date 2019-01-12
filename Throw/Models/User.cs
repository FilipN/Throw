using System;
using System.Collections.Generic;

namespace Throw.Models
{
    public partial class User
    {
        public User()
        {
            UserProject = new HashSet<UserProject>();
        }

        public int Iduser { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }

        public ICollection<UserProject> UserProject { get; set; }
    }
}
