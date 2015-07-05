using bcast.common;
using bcast.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace bcast.server.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemsController : ApiController
    {
        public ItemsController(IDataAccess dataAccess, ICacheAccess cacheAccess, ILogger logger)
        {
            DataAccess = dataAccess;
            CacheAccess = cacheAccess;
            Logger = logger;
        }
        public IDataAccess DataAccess { get; set; }
        public ICacheAccess CacheAccess { get; set; }
        public ILogger Logger { get; set; }

        /// <summary>
        /// Get count of items waiting on the endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{account}/{name}/items/count")]
        [ResponseType(typeof(int))]
        public IHttpActionResult ItemsWaiting(string account, string name)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt get item count for endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to get item count on invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                return Ok(CacheAccess.List(account, name).Length);
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while getting items waiting for endpoint '" + name + "'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get summary of items on the endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <returns>List of Items (only returns first 50 chars of data)</returns>
        [HttpGet]
        [Route("{account}/{name}/items/summary")]
        [ResponseType(typeof(List<Item>))]
        public IHttpActionResult ItemsSummary(string account, string name)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt get item summary for '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to get item summary on invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var items = (from i in GetAllItems(account, name) select new Item { ID = i.ID, DataType = i.DataType, Data = i.Data.Truncate(50) });
                return Ok(items);
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while getting items summary for endpoint '" + name + "'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get all items on the endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <returns>List of Items</returns>
        [HttpGet]
        [Route("{account}/{name}/items")]
        [ResponseType(typeof(List<Item>))]
        public IHttpActionResult AllItems(string account, string name)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt get items for '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to get items on invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var items = GetAllItems(account, name);
                return Ok(items);
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while getting items for endpoint '" + name + "'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <param name="id">The unique identifier of the item</param>
        /// <returns>Item object</returns>
        [HttpGet]
        [Route("{account}/{name}/items/{id}")]
        [ResponseType(typeof(Item))]
        public IHttpActionResult Item(string account, string name, string id)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to get item '" + id ?? "null" + "' on endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to get item '" + id ?? "null" + "' on invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var ids = CacheAccess.List(account, name);
                if (!ids.Contains(id, StringComparer.InvariantCultureIgnoreCase))
                {
                    return NotFound();
                }
                var i = CacheAccess.Get(id) as Item;
                if (i == null || i.Data == "{!db!}")
                {
                    // TODO: Get from database
                }
                return Ok(i);
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error getting item " + id + " for endpoint '" + name + "'", ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete all items on an endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{account}/{name}/items")]
        public IHttpActionResult DeleteAll(string account, string name)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt delete items for endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt delete items for invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }
            try
            {
                foreach (var id in CacheAccess.List(account, name))
                    CacheAccess.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error deleting all items for name '" + name + "'", ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <param name="id">The unique identifier of the item</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{account}/{name}/items/{id}")]
        public IHttpActionResult DeleteItem(string account, string name, string id)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt delete item '" + id ?? "null" + "' on endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt delete item '" + id ?? "null" + "' on invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var ids = CacheAccess.List(account, name);
                if (!ids.Contains(id, StringComparer.InvariantCultureIgnoreCase))
                    return NotFound();
                CacheAccess.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error deleting item " + id + " for endpoint '" + name + "'", ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("{account}/{name}/items")]
        [ResponseType(typeof(string))]
        public IHttpActionResult SendItem(string account, string name, [FromBody] SendRequest request)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to send item to endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to send item to invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            return Ok();
        }


        private List<Item> GetAllItems(string account, string dest)
        {
            var items = new List<Item>();
            foreach (var id in CacheAccess.List(account, dest))
            {
                var i = CacheAccess.Get(id) as Item;
                if (i == null || i.Data == "{!db!}")
                {
                    // TODO: Get from database
                }
                items.Add(i);
            }
            return items;
        }


    }
}
