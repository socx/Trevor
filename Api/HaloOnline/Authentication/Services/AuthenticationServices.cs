using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HaloOnline.Authentication.Services
{
    using DataAccess;
    using Domain;

    public class AuthenticationServices : IAuthenticationServices
    {
        public AuthenticationData AuthenticateByPasscode(string passcode)
        {
            var authenticationData = new AuthenticationData();

            SqlParameter[] parameters = { new SqlParameter("@Passcode", passcode)  };
            var dtSurvey = HaloDatabase.GetDataTable("dbo.rptGetPasscodeSurveys", parameters);

            if (dtSurvey.Rows.Count > 0)
            {
                authenticationData.Authenticated = true;
                authenticationData.Passcode = passcode;
            }
            else
            {
                authenticationData.Authenticated = false;
            }
            return authenticationData;
        }
    }
}