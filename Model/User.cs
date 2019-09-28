using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Throw.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public string Image { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; }

    }


}
