using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.server.Models
{
    [DataContract]
    public class SendDirectRequest
    {
        [DataMember(Name = "type")]
        public string DataType { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }
    }
}
