using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using BAL.Managers;
using Models;
using BAL.Interfaces;
using System.Web;
using System.IO;

namespace BAL.Managers
{
    public class UserManager : BaseManager, IUserManager
    {
        public UserManager(IUnitOfWork uOW)
            : base(uOW)
        {
        }

        /// <summary>
        /// Get all users from db.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return uOW.UserRepo.All.ToList();
        }
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(int? id)
        {
            if (id == null) return null;
            var user = uOW.UserRepo.GetByID(id.Value);
            if (user == null) return null;

            return user;
        }
        /// <summary>
        /// Get user by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetByEmail(string email, string password)
        {
            var user = uOW.UserRepo.All.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public User GetByEmail(string email)
        {
            var user = uOW.UserRepo.All.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public string GetEmail(int? id)
        {
            if (id == null) return null;

            var email = uOW.UserRepo.GetByID(id.Value).Email;
            return email;
        }

        /// <summary>
        /// Update user in database
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {

            uOW.UserRepo.Update(user);
            uOW.Save();

        }

        /// <summary>
        /// Insert user into database
        /// </summary>
        /// <param name="user">User</param>
        public User Insert(User user)
        {
            if (user == null) return null;

            uOW.UserRepo.Insert(user);
            uOW.Save();

            return user;
        }

        /// <summary>
        /// Check the email's existance in database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		public bool EmailIsExist(string email)
        {
            return uOW.UserRepo.All.Any(x => x.Email == email);
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int? id)
        {
            if (id == null) return;
            var user = uOW.UserRepo.GetByID(id.Value);
            uOW.UserRepo.Delete(user);
        }
        public User Find(string email, string passwrod)
        {
            return uOW.UserRepo.All.FirstOrDefault(c => c.Email == email && c.Password == passwrod);
        }
    }
}