using bcast.common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace bcast.server
{
    public class AspNetCacheAccess : ICacheAccess
    {
        private const string idprefix = "idtable|";
        private const string destprefix = "dest|";

        public string Put(string account, string[] dest, object data)
        {
            var id = Guid.NewGuid().ToString().Replace("-", "").ToLower();
            var key = idprefix + account;
            var idtable = HttpContext.Current.Cache[key] as Dictionary<string, string[]>;
            if (idtable == null)
            {
                idtable = new Dictionary<string, string[]>();
                HttpContext.Current.Cache.Add(key, idtable, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            }
            idtable.Add(id, dest);
            HttpContext.Current.Cache[key] = idtable;

            var entry = new CacheEntry() { Account = account, Dest = dest, Data = data, Timestamp = DateTime.Now };
            int lifetime = 30;
            Int32.TryParse(ConfigurationManager.AppSettings["bcast.server.cachelifetime"], out lifetime);
            HttpContext.Current.Cache.Add(id, entry, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                new TimeSpan(0, lifetime, 0), System.Web.Caching.CacheItemPriority.Default,
                onRemoveCallback);

            return id;
        }

        public string[] List(string account, string dest)
        {
            var key = idprefix + account;
            var idtable = HttpContext.Current.Cache[key] as Dictionary<string, string[]>;
            if (idtable == null)
                return null;
            var ids = new List<string>();
            foreach(var id in idtable.Keys)
            {
                var destlist = idtable[id];
                if (destlist.Length > 0 && (String.IsNullOrWhiteSpace(dest) || destlist.Contains(dest, StringComparer.InvariantCultureIgnoreCase)))
                    ids.Add(id);
            }
            return ids.ToArray();
        }

        public object Get(string id)
        {
            var entry = HttpContext.Current.Cache[id] as CacheEntry;
            if (entry == null) return null;
            return entry.Data;
        }

        public void Delete(string id, string dest)
        {
            var entry = HttpContext.Current.Cache[id] as CacheEntry;
            if (entry == null) return;

            var key = idprefix + entry.Account;
            var idtable = HttpContext.Current.Cache[key] as Dictionary<string, string[]>;
            if (idtable == null || !idtable.ContainsKey(id)) return;


            if(String.IsNullOrWhiteSpace(dest))
            {
                idtable.Remove(id);
                HttpContext.Current.Cache[key] = idtable;

                HttpContext.Current.Cache.Remove(id);
                return;
            }

            var destlist = new List<string>(idtable[id]);
            destlist.Remove(dest);
            if(destlist.Count == 0)
            {
                HttpContext.Current.Cache.Remove(key);
                HttpContext.Current.Cache.Remove(id);
                return;
            }
            idtable[id] = destlist.ToArray();
            HttpContext.Current.Cache[key] = idtable;
        }

        public Func<string, string, string[], object, bool> Persist { get; set; }

        public Func<string, bool> Remove { get; set; }

        public string Backup()
        {
            // account@id/dest,dest,dest%id/dest,dest,dest%id/dest,dest|account@id/dest,dest%id/dest,dest
            var sb = new StringBuilder();
            foreach(System.Collections.DictionaryEntry entry in HttpContext.Current.Cache)
            {
                if(entry.Key.ToString().StartsWith(idprefix))
                {
                    if (sb.Length > 0) sb.Append("|");
                    sb.Append(entry.Key.ToString().Replace(idprefix, ""));
                    sb.Append("@");
                    var idtable = entry.Value as Dictionary<string, string[]>;
                    bool first = true;
                    foreach(var id in idtable.Keys)
                    {
                        if (!first) sb.Append("%");
                        first = false;
                        sb.Append(id);
                        sb.Append("/");
                        sb.Append(String.Join(",", idtable[id]));
                    }
                }
            }

            return sb.ToString();
        }

        public void Restore(string backup)
        {
            foreach(var entry in backup.Split("|".ToCharArray()))
            {
                var entryparts = entry.Split("@".ToCharArray());
                var account = entryparts[0];
                var idtable = new Dictionary<string, string[]>();
                foreach(var table in entryparts[1].Split("%".ToCharArray()))
                {
                    var tableparts = table.Split("/".ToCharArray());
                    var id = tableparts[0];
                    var dest = tableparts[1].Split(",".ToCharArray());
                    idtable.Add(id, dest);
                }
                HttpContext.Current.Cache.Add(idprefix + account, idtable, null, Cache.NoAbsoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
        }

        private void onRemoveCallback(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            if(reason == System.Web.Caching.CacheItemRemovedReason.Removed)
            {
                if (Remove != null)
                    Remove(key);
                return;
            }

            var entry = value as CacheEntry;
            if (entry == null) return;

            if (Persist != null)
            {
                int graceperiod = 120;
                Int32.TryParse(ConfigurationManager.AppSettings["bcast.server.cachegraceperiod"], out graceperiod);
                if(!Persist(key, entry.Account, entry.Dest, entry.Data) && entry.Timestamp > DateTime.Now.AddMinutes(-1 * graceperiod))
                {
                    int lifetime = 30;
                    Int32.TryParse(ConfigurationManager.AppSettings["aspnet.server.cachelifetime"], out lifetime);
                    HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, 
                        new TimeSpan(0, lifetime, 0), System.Web.Caching.CacheItemPriority.Default, onRemoveCallback);                    
                }
            }
        }
        private class CacheEntry
        {
            public string Account { get; set; }
            public string[] Dest { get; set; }
            public object Data { get; set; }
            public DateTime Timestamp { get; set; }
        }


        public string GetDestAddr(string account, string dest)
        {
            var entry = HttpContext.Current.Cache[destprefix + account + "." + dest] as string;
            return entry ?? "";
        }

        public void PutDestAddr(string account, string dest, string addr)
        {
            HttpContext.Current.Cache.Remove(destprefix + account + "." + dest);

            int destlifetime = 360;  // 6 hours
            Int32.TryParse(ConfigurationManager.AppSettings["destlifetime"], out destlifetime);
            HttpContext.Current.Cache.Add(destprefix + account + "." + dest, addr, null, Cache.NoAbsoluteExpiration,
                new TimeSpan(0, destlifetime, 0), CacheItemPriority.Default, null);
        }
    }


}