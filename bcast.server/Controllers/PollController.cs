using bcast.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bcast.server.Controllers
{
    public class PollController : ApiController
    {
        public PollController(IDataAccess dataAccess, ICacheAccess cacheAccess, ILogger logger)
        {
            DataAccess = dataAccess;
            CacheAccess = cacheAccess;
            Logger = logger;
        }
        public IDataAccess DataAccess { get; set; }
        public ICacheAccess CacheAccess { get; set; }
        public ILogger Logger { get; set; }

        [HttpGet]
        [Route("items/{account}/count/{dest}")]
        public IHttpActionResult ItemsWaiting(string account, string dest)
        {
            try
            {
                return Ok(CacheAccess.List(account, dest).Length);
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error while getting items waiting for dest '" + dest + "'", ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("items/{account}/summary/{dest}")]
        public IHttpActionResult ItemsSummary(string account, string dest)
        {
            try
            {
                var items = (from i in GetAllItems(account, dest) select new item { id = i.id, datatype = i.datatype, data = i.data.Truncate(50) });
                return Ok(items);
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while getting items summary for dest '" + dest + "'", ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("items/{account}/all/{dest}")]
        public IHttpActionResult AllItems(string account, string dest)
        {
            try
            {
                return Ok(GetAllItems(account, dest));
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while getting all items for dest '" + dest + "'", ex);
                return InternalServerError();
            }
        }
        public class item
        {
            public string id { get; set; }
            public string datatype { get; set; }
            public string data { get; set; }
        }

        private List<item> GetAllItems(string account, string dest)
        {
            var items = new List<item>();
            foreach(var id in CacheAccess.List(account, dest))
            {
                var i = CacheAccess.Get(id) as Item;
                if(i==null || i.Data == "{!db!}")
                {
                    // TODO: Get from database
                }
                items.Add(new item() { id = i.id, datatype = i.DataType, data = i.Data });
            }
            return items;
        }

        [HttpGet]
        [Route("items/{account}/{id}/{dest}")]
        public IHttpActionResult Item(string account, string id, string dest)
        {
            try
            {
                var ids = CacheAccess.List(account, dest);
                if (!ids.Contains(id, StringComparer.InvariantCultureIgnoreCase))
                {
                    return NotFound();
                }
                var i = CacheAccess.Get(id) as Item;
                if (i == null || i.Data == "{!db!}")
                {
                    // TODO: Get from database
                }
                return Ok(new item() { id = i.id, datatype = i.DataType, data = i.Data });
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error getting item " + id + " for dest '" + dest + "'", ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("items/{account}/{dest}")]
        public IHttpActionResult DeleteAll(string account, string dest)
        {
            try
            {
                foreach (var id in CacheAccess.List(account, dest))
                    CacheAccess.Remove(id);
                return Ok();
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error deleting all items for dest '" + dest + "'", ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("items/{account}/{id}/{dest}")]
        public IHttpActionResult DeleteItem(string account, string id, string dest)
        {
            try
            {
                var ids = CacheAccess.List(account, dest);
                if (!ids.Contains(id, StringComparer.InvariantCultureIgnoreCase))
                    return NotFound();
                CacheAccess.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error deleting item " + id + " for dest '" + dest + "'", ex);
                return InternalServerError(ex);
            }
        }
    }
}
