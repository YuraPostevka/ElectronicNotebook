using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace BAL.Managers
{
    public class NotificationServiceManager : BaseManager, INotificationServiceManager
    {
        public NotificationServiceManager(IUnitOfWork uOW) : base(uOW)
        {
        }

        public class Notify
        {
            public string UserEmail { get; set; }
            public string ListName { get; set; }
            public string ItemName { get; set; }
            public DateTime? Date { get; set; }
        }

        public void GetNotifyItem(string email, string password)
        {
            var notifyItems = uOW.ToDoItemRepo.Get(includeProperties: "ToDoList.User").Where(n => n.IsNotify == true && n.NotifyTime < DateTime.UtcNow);
            foreach (var item in notifyItems)
            {
                var notifyItem = new Notify()
                {
                    UserEmail = item.ToDoList.User.Email,
                    ListName = item.ToDoList.Name,
                    ItemName = item.Text,
                    Date = item.NotifyTime
                };
                SendNotification(notifyItem, email, password);

                WriteLog("Notification was sended.");

                item.IsNotify = false;
                uOW.Save();
                return;

            }
        }

        public void SendNotification(Notify item, string email, string pass)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(email, pass);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    var from = email;
                    var to = item.UserEmail;
                    MailMessage message = new MailMessage(from, to);
                    message.Subject = "Notify item to ToDo in list: " + item.ListName + "item : " + item.ItemName;
                    message.Body = "";

                    client.Send(message);

                }
            }

            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }
        }



        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.UtcNow.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public static void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.UtcNow.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
