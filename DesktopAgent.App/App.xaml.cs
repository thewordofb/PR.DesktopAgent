using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopAgent.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;

        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = new string[] { };
            Task t = Task.Run(() => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().Run());

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (e.ApplicationExitCode == 99)
            {
                base.OnExit(e);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //add some bootstrap or startup logic 
            window = new MainWindow();

            window.Show();
        }
    }
}
