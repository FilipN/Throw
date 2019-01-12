using System;
using System.Collections.Generic;

namespace Throw.Models
{
    public partial class ErrorLog
    {
        public int Iderror { get; set; }
        public string Component { get; set; }
        public string Function { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
    }
}
