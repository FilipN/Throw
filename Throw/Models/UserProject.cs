using System;
using System.Collections.Generic;

namespace Throw.Models
{
    public partial class UserProject
    {
        public int Iduser { get; set; }
        public int Idproject { get; set; }
        public bool Role { get; set; }
        public DateTime? Accessed { get; set; }
        public DateTime? Modified { get; set; }

        public Project IdprojectNavigation { get; set; }
        public User IduserNavigation { get; set; }
    }
}
