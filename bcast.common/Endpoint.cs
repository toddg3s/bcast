using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
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
        public EndpointType Type { get; set; }
        public string Location { get; set; }
        public Uri LocationUri { get { return new Uri(Location); } set { Location = value.ToString(); } }
        public bool Enabled { get; set; }
        public bool AllCast { get; set; }
        public bool Default { get; set; }

        public string AccountName { get; set; }
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
