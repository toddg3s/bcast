using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using bcast.common;
using System.Text.RegularExpressions;
using System.Configuration;

namespace bcast.server.Controllers
{
    public class SendController : ApiController
    {
        public SendController(IDataAccess dataAccess, ICacheAccess cacheAccess, ILogger logger)
        {
            DataAccess = dataAccess;
            CacheAccess = cacheAccess;
            Logger = logger;
        }
        public IDataAccess DataAccess { get; set; }
        public ICacheAccess CacheAccess { get; set; }
        public ILogger Logger { get; set; }

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

            if(req.Destination.Length == 1 && req.Destination[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: add parameters for selecting enabled only and allcast only
                req.Destination = DataAccess.getEndpointList(req.Sec.account);
            }

            var polldest = new List<string>();
            foreach(var dest in req.Destination)
            {
                var addr = CacheAccess.GetDestAddr(req.Sec.account, dest);
                if(String.IsNullOrWhiteSpace(addr))
                {
                    var endp = DataAccess.getEndpoint(req.Sec.account, dest);
                    addr = (String.IsNullOrWhiteSpace(endp.Location)) ? "poll" : endp.Location;
                    CacheAccess.PutDestAddr(req.Sec.account, dest, addr);
                }
                if(addr=="poll" && !polldest.Contains(dest, StringComparer.InvariantCultureIgnoreCase))
                {
                    polldest.Add(dest);
                }
                else
                {
                    // TODO: Formulate direct request to addr
                }
            }

            if(polldest.Count > 0)
            {
                int maxcachedatasize = 1024;
                Int32.TryParse(ConfigurationManager.AppSettings["bcast.server.maxcachedatasize"], out maxcachedatasize);
                if(req.Data.Length <= maxcachedatasize)
                {
                    CacheAccess.Put(req.Sec.account, polldest.ToArray(), req.Data);
                }
                else
                {
                    CacheAccess.Put(req.Sec.account, polldest.ToArray(), "{!db!}");
                    // TODO: call data access method to store data in Items table;
                }
            }
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
            var dt = "text";
            var d = data.Trim().ToLower();
            if(d.Length < 250)
            {
                if (d.StartsWith("http") || d.Contains("www.") || Regex.IsMatch(d, "weburl_regex_pattern"))
                {
                    return "hyperlink";
                }
                if (d.StartsWith("mailto") || Regex.IsMatch(d, "email_regex_pattern"))
                {
                    return "email";
                }
            }

            if(d.StartsWith("begin:vcard"))
            {
                return "contact/vcard";
            }
            if(d.StartsWith("<?xml"))
            {
                // TODO: Check for specific schemas

                return "xml";
            }


            return dt;
        }

    }
}
