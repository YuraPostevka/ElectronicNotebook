using BAL.Interfaces;
using BAL.Managers;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NotificationService
{
    public partial class Scheduler : ServiceBase
    {
        private int sleepTime = 30000;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //set only one thread(ex:  task.sleep(5sec))
            var t = Task.Run(() =>
            {

                while (!tokenSource.Token.IsCancellationRequested)
                {
                    CheckAndSendNotifications();
                    Thread.Sleep(sleepTime);
                }

            }, tokenSource.Token);
        }

        private void CheckAndSendNotifications()
        {
            var email = ConfigurationManager.AppSettings["Email"];
            var pass = ConfigurationManager.AppSettings["Password"];
            try
            {
                var serviceManager = new NotificationServiceManager(new UnitOfWork());
                //send notification
                serviceManager.GetNotifyItem(email, pass);
            }
            catch (Exception ex)
            {
                NotificationServiceManager.WriteErrorLog(ex);
            }
        }

        protected override void OnStop()
        {
            tokenSource.Cancel();
            NotificationServiceManager.WriteLog("Service stopped.");
        }
    }
}
