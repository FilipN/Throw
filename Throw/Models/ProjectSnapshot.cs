using System;
using System.Collections.Generic;

namespace Throw.Models
{
    public partial class ProjectSnapshot
    {
        public int Idsnapshot { get; set; }
        public int Idproject { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public Project IdprojectNavigation { get; set; }
    }
}
