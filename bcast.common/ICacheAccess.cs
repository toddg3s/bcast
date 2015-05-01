using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bcast.common
{
    public interface ICacheAccess
    {
        string Put(string account, string[] dest, object data);
        string[] List(string account, string dest);
        object Get(string id);
        void Delete(string id, string dest);
        Func<string, string, string[], object, bool> Persist { get; set; }
        Func<string, bool> Remove { get; set; }
        string Backup();
        void Restore(string backup);
    }
}
