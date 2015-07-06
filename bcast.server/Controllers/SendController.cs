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
    public class SendController : ApiController
    {
        [HttpGet]
        [Route("{account}/{name}/send")]
        [ResponseType(typeof(string))]
        public IHttpActionResult SendSimple(string account, string name, [FromUri] string dest, [FromUri] string data)
        {
            return Ok();
        }

        [HttpPost]
        [Route("{account}/{name}/send")]
        public IHttpActionResult Send(string account, string name, [FromBody] SendRequest request)
        {
            return Ok();
        }
    }
}
