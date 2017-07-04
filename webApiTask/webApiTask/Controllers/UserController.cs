using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using webApiTask.ActionFilters;
using webApiTask.Providers;
using Microsoft.AspNet.Identity;
using webApiTask.Models;
using System.Security.Claims;
using BAL.Interfaces;

namespace webApiTask.Controllers
{
    /// <summary>
    /// User
    /// </summary>
    public class UserController : ApiController
    {
        private IUserManager userManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        ///
        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        [ResponseType(typeof(List<User>))]
        public IHttpActionResult GetAll()
        {
            try
            {
                var users = userManager.GetAll();
                if (users != null) { return Ok(users); }
                else { return NotFound(); }
            }
            catch
            {
                return NotFound();
            }

        }
        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetById(int? id)
        {
            try
            {
                var user = userManager.GetById(id);
                if (user != null) { return Ok(user); }
                else { return NotFound(); }
            }
            catch
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Insert user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: api/User
        public IHttpActionResult Post(User user)
        {
            try
            {
                userManager.Insert(user);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        // PUT: api/User/5
        public IHttpActionResult Put(User user)
        {
            try
            {
                userManager.Update(user);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                userManager.Delete(id);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
