using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnline.Authentication.Services
{
    using Domain;

    public interface IAuthenticationServices
    {
        AuthenticationData AuthenticateByPasscode(string passcode);
    }
}
