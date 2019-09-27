using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Throw.Model;

namespace Throw.SessionData
{
    public class Code
    {
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public int NumberOfLine { get; set; }
        public string Content { get; set; }

        public DateTime LastModified { get; set; }

        public User UserModifed { get; set; }
    }
}
