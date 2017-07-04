using BAL;
using BAL.Interfaces;

using BAL.Managers;
using DAL;
using DAL.Interfaces;

using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using webApiTask.Helpers;

namespace webApiTask
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Container container;
        protected void Application_Start()
        {
            InjectorContainer();
            AutoMapperConfig.Configure();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

        }
        private void InjectorContainer()
        {
            try
            {
                container = new Container();
                container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

                container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
                container.Register<IUserManager, UserManager>();
                container.Register<IToDoListManager, ToDoListManager>();
                container.Register<IToDoItemManager, ToDoItemManager>();
                container.Register<IInviteUserManager, InviteUserManager>();
                container.Register<ITagManager, TagManager>();
                container.Register<INotificationServiceManager, NotificationServiceManager>();
                container.Register<IMapManager, MapManager>();

                container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

                GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

                DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
