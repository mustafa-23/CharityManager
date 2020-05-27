using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace CharityManager.Service
{
    public class CharityWindowsService : ServiceBase
    {
        ServiceHost service = null;
        public CharityWindowsService()
        {
            InitializeComponent();
            ServiceName = "CharityService";
        }
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            try
            {
                if (service != null)
                    service.Close();
                service = new ServiceHost(typeof(Charity));
                service.Open();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (service != null)
                {
                    service.Abort();
                    service = null;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void Main()
        {
            Run(new CharityWindowsService());
        }

        private void InitializeComponent()
        {

        }
    }
}
