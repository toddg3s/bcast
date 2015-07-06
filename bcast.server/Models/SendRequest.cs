﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.server.Models
{
    [DataContract]
    public class SendRequest : SendDirectRequest
    {
        [DataMember(Name = "dest")]
        public string[] Destination { get; set; }
    }
}
