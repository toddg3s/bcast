using bcast.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace bcast.server.Controllers
{
    public class SendController : ApiController
    {
        [HttpGet]
        [Route("send/{id}")]
        public void Simple(string id, [FromUri] string data)
        {
            Unsecure(id, new Request() { Destination=new string[] { "all" }, Data = data});
        }

        [HttpPut]
        [Route("send/{id}")]
        public void Unsecure(string id, [FromBody] Request request)
        {
            if (request.Sec == null)
                request.Sec = new SecurityContext() { id = id };

            ProcessRequest(request);
        }

        [HttpPut]
        [Route("send/secure")]
        public void Secure([FromBody] Request request)
        {
            ProcessRequest(request);
        }

        private void ProcessRequest(Request req)
        {
            if(req.Sec==null)
                throw new Exception("Request cannot be processed.  Missing security context.");

            if (String.IsNullOrWhiteSpace(req.Source))
                req.Source = GuessSource(req.Sec.id, HttpContext.Current);

            if (String.IsNullOrWhiteSpace(req.DataType))
                req.DataType = GuessDataType(req.Sec.id, req.Data);

            if (req.Destination == null || req.Destination.Length == 0)
                req.Destination = new string[] { "default" };

        }

        private string GuessSource(string id, HttpContext httpContext)
        {
            var source = "";

            if(!String.IsNullOrWhiteSpace(id))
            {

            }


            return source;
        }

        private string GuessDataType(string id, string data)
        {
            throw new NotImplementedException();
        }

    }
}
