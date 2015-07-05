using bcast.authclient;
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
    /// Endpoints to manage bcast accounts
    /// </summary>
    public class AccountController : ApiController
    {
        private IDataAccess _data;
        private ICacheAccess _cache;
        private ILogger _log;

        public AccountController(IDataAccess dataAccess, ICacheAccess cacheAccess, ILogger logger)
        {
            _data = dataAccess;
            _cache = cacheAccess;
            _log = logger;
        }

        /// <summary>
        /// Get information about an existing account
        /// </summary>
        /// <param name="account">The name of the account</param>
        /// <returns>Account object</returns>
        [HttpGet]
        [Route("{account}")]
        [ResponseType(typeof(Account))]
        public IHttpActionResult GetAccount(string account)
        {
            // TODO: This is totally unsecure right now.  Add code to validate that the caller is allowed to get data for this account
            if(!account.IsValidAccount())
            {
                _log.Debug(account, "Attempt to get an invalid account");
                return NotFound();
            }
            try
            {
                var acct = _data.getAccount(account);
                if (acct == null)
                {
                    _log.Debug(account, "Attempt to get a non-existent account");
                    return NotFound();
                }
                _log.Debug(account, "Account accessed");
                return Ok(acct);
            }
            catch(Exception ex)
            {
                _log.Error(account, "Error occurred while getting account", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Create a new account
        /// </summary>
        /// <param name="account">The name of the new account</param>
        /// <param name="request">A request object with the email and password for the new account</param>
        /// <returns>Token that can be used to make further calls</returns>
        [HttpPost]
        [Route("{account}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult CreateAccount(string account, [FromBody] AccountCreateRequest request)
        {
            if(!account.IsValidAccount())
            {
                _log.Debug(account,"Attempt to create an invalid account");
                return BadRequest("Invalid account name.  Must be alphanumeric, max 128 chars");
            }
            if(!request.Email.IsValidEmail())
            {
                _log.Debug(account, "Attempt to create account with invalid email '" + request.Email ?? "null" + "'");
                return BadRequest("Invalid email address specified");
            }
            if(!request.Password.IsValidPassword())
            {
                _log.Debug(account, "Attempt to create account with invalid password");
                return BadRequest("Invalid password. Must be 8-32 characters with at least one upper, lower and number.");
            }
            try
            {
                var acct = _data.getAccount(account);
                if (acct != null)
                {
                    return Conflict();
                }
            }
            catch(Exception ex)
            {
                _log.Error(account, "Error while attempting to check if the account exists", ex);
            }
            var token = "";
            try
            {
                token = Auth.Create(account, request.Password);
                _data.saveAccount(new Account { Name = account, Email = request.Email });
                return Ok(token);
            }
            catch(Exception ex)
            {
                _log.Error(account, "Error while creating account", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Epdate an existing account
        /// </summary>
        /// <param name="account">The name of the account to update</param>
        /// <param name="request">Request object specifying the new values for the account</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{account}")]
        public IHttpActionResult UpdateAccount(string account, [FromBody] AccountUpdateRequest request)
        {
            if (!account.IsValidAccount()) 
            {
                _log.Debug(account, "Attempt to update an invalid account");
                return NotFound();
            }
            if (!request.Email.IsValidEmail())
            {
                _log.Debug(account, "Attempt to update account with invalid email '" + request.Email ?? "null" + "'");
                return BadRequest();
            }
            try
            {
                var acct = _data.getAccount(account);
                if(acct==null)
                {
                    _log.Debug(account, "Attempt to update a non-existent account");
                    return NotFound();
                }
                acct.Email = request.Email;
                _data.saveAccount(acct);
                return Ok();
            }
            catch(Exception ex)
            {
                _log.Error(account, "Error while updating account", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Delete an existing endpoint
        /// </summary>
        /// <param name="account">The name of the accuont to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{account}")]
        public IHttpActionResult DeleteAccount(string account)
        {
            if(!account.IsValidAccount())
            {
                _log.Debug(account, "Attempt to delete an invalid account");
                return NotFound();
            }
            try
            {
                var acct = _data.getAccount(account);
                if(acct==null)
                {
                    _log.Debug(account, "Attempt to update a non-existent account");
                    return NotFound();
                }

                // TODO: Add call to delete function, when implemented
                return Ok();
            }
            catch(Exception ex)
            {
                _log.Error(account, "Error while deleting account", ex);
                return InternalServerError();
            }
        }
    }
}
