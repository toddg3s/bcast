using bcast.auth.Models;
using bcast.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bcast.auth.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(IAuthData dataAccess)
        {
            DataAccess = dataAccess;
        }
        public IAuthData DataAccess { get; set; }

        [HttpPost]
        [Route("auth/check")]
        public IHttpActionResult Check([FromBody] Credentials credentials)
        {
            try
            {
                var acct = DataAccess.getAccount(credentials.Name);
                if(acct.Password == credentials.Password)
                {
                    // TODO: generate token
                    return Ok<string>("token");
                }
                return Unauthorized();
            }
            catch(AccountNotFoundException)
            {
                return Unauthorized();
            }
            catch(Exception)
            {
                // TODO: log error
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("auth/create")]
        public IHttpActionResult Create([FromBody] Credentials credentials)
        {
            try
            {
                DataAccess.createAccount(credentials.Name, credentials.Password);
            }
            catch(AccountAlreadyExistsException aex)
            {
                return BadRequest(aex.Message);
            }
            catch(Exception ex)
            {
                // TODO: log error
                return InternalServerError();
            }
            // TODO: generate token
            return Ok<string>("token");
        }

        [HttpPost]
        [Route("auth/reset")]
        public IHttpActionResult Reset([FromBody] Credentials credentials)
        {
            try
            {
                DataAccess.changePassword(credentials.Name, credentials.Password);
            }
            catch (AccountNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                // TODO: log error
                return InternalServerError();
            }
            // TODO: generate token
            return Ok<string>("token");
        }
    }
}
