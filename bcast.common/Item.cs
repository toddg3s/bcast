using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    [DataContract]
    public class Item
    {
        [DataMember(Name="id")]
        public string ID { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }
        [DataMember(Name="type")]
        public string DataType { get; set; }
        [DataMember(Name="data")]
        public string Data { get; set; }
    }
}
