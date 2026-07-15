using System.ComponentModel;

namespace EDDY.Nexus.WindowsService.DeliveryEngine
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
