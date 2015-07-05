using bcast.common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bcast.web.Models
{
    public class EndpointsModel
    {
        public string[] EndpointNames { get; set; }
 
    }

    public class EndpointModel : Endpoint
    {
        public EndpointModel()
        {
            mEndpointTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) { 
                { "Windows",  "Windows computer"},
                { "Mac",      "Mac computer (not iPad/iPhone/iPod)"},
                { "Linux",    "Linux computer"},
                { "IOS",      "iOS device (iPhone/iPad/iPod)"},
                { "Android",  "Android device (Phone, Kindle, etc.)"},
                { "WinPhone", "Windows phone"},
                { "Other",    "Other unknown device"}
            };
        }
        public EndpointModel(string AccountName)
            : base()
        {
            this.AccountName = AccountName;
        }
        [Required]
        [Display(Name = "Name", Description = "The unique name of this endpoint.  Something like 'iPhone' or 'HomeComputer'.  The name must contain only letters and numbers.")]
        [StringLength(128, ErrorMessage = "The {0} must be between 3 and 128 characters long.", MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "The {0} must only have letters and numbers.")]
        public string mItemName { get { return Name; } set { Name = value; } }

        [Required]
        [Display(Name = "Type", Description = "The type of device this is.")]
        public string mType
        {
            get
            {
                return mEndpointTypes[Enum.GetName(typeof(EndpointType), Type)];
            }

            set
            {
                if (mEndpointTypes.ContainsKey(value))
                {
                    Type = (EndpointType)Enum.Parse(typeof(EndpointType), value);
                    return;
                }
                foreach (var key in mEndpointTypes.Keys)
                {
                    if (mEndpointTypes[key].Equals(value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Type = (EndpointType)Enum.Parse(typeof(EndpointType), key);
                        return;
                    }
                }
            }
        }
        public Dictionary<string, string> mEndpointTypes { get; private set; }

        [Required]
        [Display(Name = "Communication Type", Description="The way that bcast will talk to the device.  Push means that bcast will send a message directly to the device.  Pull means the device will check for messages regularly")]
        public string Communication
        {
            get { return (LocationUri.Scheme.Equals("pull", StringComparison.InvariantCultureIgnoreCase)) ? "pull" : "push"; }
            set
            {
                if (value.Trim().Equals("pull", StringComparison.InvariantCultureIgnoreCase))
                    LocationUri = new Uri("pull://");
                else
                {
                    if (value.Trim().Equals("push", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if(LocationUri.Scheme.Equals("pull", StringComparison.InvariantCultureIgnoreCase))
                        {
                            LocationUri = new Uri("unknown://");
                        }
                    }
                    else
                        throw new ArgumentException("Communication must be 'pull' or 'push'", "value");
                }
            }
        }

        [Display(Name="HTTP address")]
        public string HttpUrl { get; set; }

        [Display(Name="HTTP method", Description="GET, POST, PULL, etc.")]
        public string HttpMethod { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Provider")]
        public string PhoneProvider { get; set; }

    }
}