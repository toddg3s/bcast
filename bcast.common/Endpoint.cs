using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    [DataContract(Name="endpoint")]
    public class Endpoint
    {
        public string Name
        {
            get
            {
                return (AccountName ?? "") + "." + (ItemName ?? "");
            }
            set
            {
                if(value==null)
                {
                    AccountName = null;
                    ItemName = null;
                    return;
                }
                var parts = value.Split(".".ToCharArray());
                AccountName = parts[0];
                ItemName = (parts.Length > 1) ? parts[1] : "";
            }
        }
        [DataMember(Name="type")]
        public EndpointType Type { get; set; }
        [DataMember(Name="location")]
        public string Location { get; set; }
        public Uri LocationUri { get { try { return new Uri(Location); } catch { return null; } } set { Location = value.ToString(); } }
        [DataMember(Name="enabled")]
        public bool Enabled { get; set; }
        [DataMember(Name="allcast")]
        public bool AllCast { get; set; }
        [DataMember(Name="default")]
        public bool Default { get; set; }
        [DataMember(Name="account")]
        public string AccountName { get; set; }
        [DataMember(Name="name")]
        public string ItemName { get; set; }

        public Endpoint() { Enabled = true; AllCast = true; }
    }

    public enum EndpointType
    {
        Windows,
        Mac,
        Linux,
        IOS,
        Android,
        WinPhone,
        Other
    }
}
