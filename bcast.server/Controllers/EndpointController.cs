using bcast.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bcast.server.Controllers
{
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

        [HttpGet]
        [Route("endpoint/{account}/{name}")]
        public IHttpActionResult GetEndpoint(string account, string name)
        {
            if(!account.IsValidAccount())
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            if (!name.IsValidName())
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");
            try
            {
                var endp = DataAccess.getEndpoint(account.EndpointName(name));
                if(endp==null)
                {
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

        [HttpPost]
        [Route("endpoint/{account}/{name}")]
        public IHttpActionResult AddEndpoint(string account, string name, [FromBody] Endpoint endp)
        {
            if (!account.IsValidAccount())
                return BadRequest("account value is not valid (alphanumeric, max 128 chars)");
            if (!name.IsValidName())
                return BadRequest("name value is not valid (alphanumeric, max 128 chars)");

            try
            {
                var checkendp = DataAccess.getEndpoint(account.EndpointName(name));
                if(checkendp!=null)
                {
                    return Conflict();
                }
            }
            catch(Exception ex)
            {
                Logger.Error(account, "Error while checking endpoint '" + name + "' to add", ex);
                return InternalServerError();
            }

            return Ok();
        }
    }
}
