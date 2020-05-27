using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CharityManager.Service
{
    [RunInstaller(true)]
    public class CharityInstaller : Installer
    {
        ServiceProcessInstaller process;
        ServiceInstaller service;

        public CharityInstaller()
        {
            process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            service = new ServiceInstaller
            {
#if DEBUG
                ServiceName = "Charity Debug",
#else
                ServiceName = "Charity",
#endif
            };
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
