using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HaloOnline.Authentication.Rest
{
    using Common;
    using Domain;
    using Services;

    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiController
    {
        private IAuthenticationServices _authenticationServices = new AuthenticationServices();
        private ILogger _fileLogger = new FileLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passcode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("login/{passcode}")]
        [Route("authenticate/{passcode}")]
        [Route("~/api/reports/login/{passcode}")]
        public IHttpActionResult Authenticate(string passcode)
        {
            try
            {
                var authenticationData = _authenticationServices.AuthenticateByPasscode(passcode);
                if (authenticationData != null)
                {
                    authenticationData.ExpiryDate = DateTime.Now.AddHours(1);
                    return Ok(authenticationData);
                }
            }
            catch (Exception ex)
            {
                _fileLogger.LogError(ex);
            }
            return Content(HttpStatusCode.OK, "Invalid Passcode");
        }
    }
}
