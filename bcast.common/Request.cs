﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class Request : Item
    {
        public SecurityContext Sec { get; set; }
        public bool Immediate { get; set; }
        public string Source { get; set; }
        public string[] Destination { get; set; }
    }
}
