using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Models;

namespace BAL.Managers
{
    public class InviteUserManager : BaseManager, IInviteUserManager
    {
        public InviteUserManager(IUnitOfWork uOW) : base(uOW)
        {
        }

        public void CreateInvite(InviteUser user)
        {
            var guidId = Guid.NewGuid();

            user.GuidId = guidId.ToString();

            uOW.InviteUserRepo.Insert(user);
            uOW.Save();

            //send invite to "mail"

        }

        public InviteUser GetByGuid(string guidId)
        {
            var user = uOW.InviteUserRepo.All.Where(g => g.GuidId == guidId).FirstOrDefault();

            return user;
        }
    }
}
