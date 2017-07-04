using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IInviteUserManager
    {
        void CreateInvite(InviteUser user);
        InviteUser GetByGuid(string guidId);
    }
}
