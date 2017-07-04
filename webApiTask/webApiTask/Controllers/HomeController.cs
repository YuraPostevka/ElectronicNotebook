using BAL.Interfaces;
using BAL.Managers;
using DAL;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using webApiTask.Models;
using Microsoft.AspNet.Identity;
using Models;
using webApiTask.Helpers;
using System.Configuration;
using System.IO;
using System.Web.Http.Results;
using Models.DTO;

namespace webApiTask.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>

    public class HomeController : BaseController
    {
        private IToDoItemManager itemManager;
        private IToDoListManager listManager;
        private IUserManager userManager;
        private IInviteUserManager inviteUserManager;
        private ITagManager tagManager;
        private IAuthenticationManager authManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemManager"></param>
        /// <param name="listManager"></param>
        /// <param name="userManager"></param>
        public HomeController(IToDoItemManager itemManager, IToDoListManager listManager, IUserManager userManager, IInviteUserManager inviteUserManager, ITagManager tagManager)
        {
            this.itemManager = itemManager;
            this.listManager = listManager;
            this.userManager = userManager;
            this.tagManager = tagManager;
            this.inviteUserManager = inviteUserManager;
        }

        public ActionResult Discussion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Map()
        {
            return View(new ListTagDTO());
        }

        public ActionResult ExportToExcel()
        {
            var exportToExcel = new ExportToExcel();

            var lists = listManager.GetAll();
            exportToExcel.WriteToExcel(lists);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult InviteUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult InviteUser(InviteUser user)
        {
            inviteUserManager.CreateInvite(user);
            return RedirectToAction("Index", "Home");
        }

        //GET: localhost/webApi/RegisterInviteUser/id
        [HttpGet]
        public ActionResult RegisterInviteUser(string id)
        {
            var inviteUser = inviteUserManager.GetByGuid(id);
            if (inviteUser == null) { return HttpNotFound(); }

            var userDb = userManager.GetByEmail(inviteUser.Email);
            if (userDb == null)
            {
                userDb = new User()
                {
                    Email = inviteUser.Email
                };
                return View(userDb);
            }

            return View(userDb);
        }

        [HttpPost]
        public ActionResult RegisterInviteUser(User user)
        {
            userManager.Insert(user);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Photo()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null) { return null; }

            var imgHelper = new ImageHelper();
            var imgPath = imgHelper.GetImage(Convert.ToInt32(userId));

            return View(new UserModel() { ProfileImgUrl = imgPath });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfiguringUser()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = userManager.GetById(Convert.ToInt32(id));
            return View(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Profile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfiguringUser(User user, string Profile, HttpPostedFileBase image, string points, string zoom)
        {
            var imgHelper = new ImageHelper();
            //imgHelper.CropAndSaveImage(user.Id, image, points, zoom);
            imgHelper.SaveImage(user.Id, Profile);


            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }



        /// <summary>
        /// Login page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginBindingModel();
            return View(model);
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginBindingModel model)
        {

            var userManager = new UserManager(new UnitOfWork());
            var userDb = userManager.Find(model.Email, model.Password);

            if (userDb == null) return View("Error");

            ApplicationUser user = new ApplicationUser()
            {
                Id = userDb.Id.ToString(),
                UserName = userDb.Email,
                Email = userDb.Email
            };

            //ClaimsIdentity claim = new ClaimsIdentity(OAuthDefaults.AuthenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            //claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
            //claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email, ClaimValueTypes.String));
            //claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
            //    "OWIN Provider", ClaimValueTypes.String));


            ClaimsIdentity cookiesIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            cookiesIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
            cookiesIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email, ClaimValueTypes.String));
            cookiesIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

            AuthenticationProperties properties = CreateProperties(user.UserName);
            //AuthenticationTicket ticket = new AuthenticationTicket(claim, properties);


            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, cookiesIdentity);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            authManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.Response.Cookies.Set(new HttpCookie("token") { Value = string.Empty });
            return RedirectToAction("Index", "Home");
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

    }
}
