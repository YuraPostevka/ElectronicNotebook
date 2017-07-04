using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Managers;
using DAL;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(webApiTask.Startup))]

namespace webApiTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        private async Task<IEnumerable<Claim>> Authenticate(string username, string password)
        {
            // authenticate user
            var userManager = new UserManager(new UnitOfWork());
            var userDb = userManager.Find(username, password);






            return null;
        }
    }
}
