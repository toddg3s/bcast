using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class Request : ItemBase
    {
        public SecurityContext Sec { get; set; }
        public string Source { get; set; }
        public string[] Destination { get; set; }
    }
}
