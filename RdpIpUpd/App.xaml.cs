using System.Windows;

namespace RdpIpUpd
{
    public partial class App : Application
    {
        private AppBootstrap _bootstrap;

        public App()
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _bootstrap = new AppBootstrap();
           
        }
    }
}