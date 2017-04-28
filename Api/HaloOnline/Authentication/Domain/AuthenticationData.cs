using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Authentication.Domain
{
    public class AuthenticationData
    {
        public string Username { get; set; }
        public string Passcode { get; set; }
        public bool Authenticated { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}