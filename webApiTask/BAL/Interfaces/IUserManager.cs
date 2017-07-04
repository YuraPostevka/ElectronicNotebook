using Models;
using System.Collections.Generic;
using System.Web;

namespace BAL.Interfaces
{
    public interface IUserManager
    {
        void Update(User user);
        User GetById(int? id);
        User GetByEmail(string email, string password);
        User GetByEmail(string email);
        List<User> GetAll();
        User Insert(User user);
        string GetEmail(int? Id);
        bool EmailIsExist(string email);
        void Delete(int? id);
        User Find(string email, string passwrod);

    }
}