using HaloOnline.Authentication.Services;
using HaloOnline.Common;
using HaloOnline.Reports.Services;
using HaloOnline.ViewData.Services;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace HaloOnline
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IAuthenticationServices, AuthenticationServices>();
            container.RegisterType<IViewDataServices, ViewDataServices>();
            container.RegisterType<IReportsServices, ReportsServices>();
            container.RegisterType<ILogger, FileLogger>();
                        
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}