using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    [DataContract]
    public class Account
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name="email")]
        public string Email { get; set; }
        [DataMember(Name="locked")]
        public bool Locked { get; set; }
        [IgnoreDataMember]
        public string ResetCode { get; set; }
    }
}
