using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class Request
    {
        public SecurityContext Sec { get; set; }
        public string Source { get; set; }
        public string[] Destination { get; set; }
        public bool Immediate { get; set; }
        public string DataType { get; set; }
        public string Data { get; set; }
    }
}
