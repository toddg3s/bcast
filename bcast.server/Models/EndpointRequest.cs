using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.server.Models
{
    [DataContract]
    public class EndpointRequest
    {
        [DataMember(Name = "type")]
        public string EndpointType { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "enabled")]
        public bool? Enabled { get; set; }
        [DataMember(Name = "allcast")]
        public bool? AllCast { get; set; }
        [DataMember(Name = "default")]
        public bool? Default { get; set; }
    }
}
