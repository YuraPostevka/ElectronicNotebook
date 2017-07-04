using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BAL.Managers.NotificationServiceManager;

namespace BAL.Interfaces
{
    public interface INotificationServiceManager
    {
        void GetNotifyItem(string mail, string password);
        void SendNotification(Notify item, string mail, string password);
    }
}
