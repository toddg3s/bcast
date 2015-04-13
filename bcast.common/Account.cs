using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public class Account
    {
        public Guid Uid { get; set; }
        public string UserName { get; set; }
        public bool Locked { get; set; }
        public List<Endpoint> Endpoints { get; set; }

    }
}
