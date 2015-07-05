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
    public class EndpointController : ApiController
    {
        public EndpointController(IDataAccess dataAccess, ICacheAccess cacheAccess, ILogger logger)
        {
            DataAccess = dataAccess;
            CacheAccess = cacheAccess;
            Logger = logger;
        }

        public IDataAccess DataAccess { get; set; }
        public ICacheAccess CacheAccess { get; set; }
        public ILogger Logger { get; set; }

        /// <summary>
        /// Get information about an existing endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint</param>
        /// <returns>Endpoint object</returns>
        [HttpGet]
        [Route("{account}/{name}")]
        [ResponseType(typeof(Endpoint))]
        public IHttpActionResult GetEndpoint(string account, string name)
        {
            if(!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to update endpoint '" + name + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to update invalid endpoint '" + name + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }
            try
            {
                var endp = DataAccess.getEndpoint(account.EndpointName(name));
                if(endp==null)
                {
                    Logger.Debug(account, "Attempt to access non-existent endpoint '" + name + "'");
                    return NotFound();
                }
                return Ok(endp);
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error while getting endpoint '" + name + "'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Add a new endpoint
        /// </summary>
        /// <param name="account">The name of the account that will own the new endpoint</param>
        /// <param name="name">The name of the new endpoint</param>
        /// <param name="endp">Request object containing data about the new endpoint</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{account}/{name}")]
        public IHttpActionResult AddEndpoint(string account, string name, [FromBody] EndpointRequest endp)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to add endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to update invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var checkendp = DataAccess.getEndpoint(account.EndpointName(name));
                if(checkendp!=null)
                {
                    Logger.Debug(account, "Attempt to add endpoint '" + name + "' that already exists");
                    return Conflict();
                }
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error while checking endpoint '" + name + "' to add", ex);
                return InternalServerError();
            }
            try
            {
                DataAccess.saveEndpoint(new Endpoint()
                {
                    AccountName = account,
                    Name = name,
                    Type = endp.EndpointType.ToEndpointType(),
                    Location = endp.Location,
                    Enabled = (endp.Enabled.HasValue) ? endp.Enabled.Value : false,
                    AllCast = (endp.AllCast.HasValue) ? endp.AllCast.Value : false,
                    Default = (endp.Default.HasValue) ? endp.Default.Value : false
                });
                return Ok();
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error while attempting to create endpoint '" + name + "'", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Update an endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint to update</param>
        /// <param name="endp">Request object containing updated data for the endpoint</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{account}/{name}")]
        public IHttpActionResult UpdateEndpoint(string account, string name, [FromBody] EndpointRequest endp)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to add endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to update invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var e = DataAccess.getEndpoint(account.EndpointName(name));
                if (e == null)
                {
                    Logger.Debug(account, "Attempt to update non-existent endpoint '" + name + "'");
                    return NotFound();
                }
                if (!String.IsNullOrWhiteSpace(endp.EndpointType))
                    e.Type = endp.EndpointType.ToEndpointType();
                if (!String.IsNullOrWhiteSpace(endp.Location))
                    e.Location = endp.Location;
                if (endp.Enabled.HasValue)
                    e.Enabled = endp.Enabled.Value;
                if (endp.AllCast.HasValue)
                    e.AllCast = endp.AllCast.Value;
                if (endp.Default.HasValue)
                    e.Default = endp.Default.Value;

                DataAccess.saveEndpoint(e);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while updating endpoint '" + name + "'", ex);
                return InternalServerError();
            }

        }

        /// <summary>
        /// Delete an existing endpoint
        /// </summary>
        /// <param name="account">The name of the account that owns the endpoint</param>
        /// <param name="name">The name of the endpoint to update</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{account}/{name}")]
        public IHttpActionResult DeleteEndpoint(string account, string name)
        {
            if (!account.IsValidAccount())
            {
                Logger.Debug(account, "Attempt to add endpoint '" + name ?? "null" + "' on invalid account");
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            }
            if (!name.IsValidName())
            {
                Logger.Debug(account, "Attempt to update invalid endpoint '" + name ?? "null" + "'");
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            }

            try
            {
                var checkendp = DataAccess.getEndpoint(account.EndpointName(name));
                if (checkendp == null)
                {
                    Logger.Debug(account, "Attempt to delete non-existent endpoint '" + name + "'");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account, "Error while checking endpoint '" + name + "' to delete", ex);
                return InternalServerError();
            }

            // TODO: Add call to delete endpoint, when implemented
            return Ok();
        }
    }
}
