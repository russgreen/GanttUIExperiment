using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GantUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int projectID;

        void app_Startup(object sender, StartupEventArgs e)
        {
            //Register Syncfusion license
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense();

            //process the args
            var pidArg = "/pid=";

            // If no command line arguments were provided, don't process them if (e.Args.Length == 0) return;  
            if (e.Args.Length > 0)
            {
                foreach (var arg in e.Args)
                {
                    if (arg.StartsWith(pidArg, StringComparison.InvariantCultureIgnoreCase))
                    {
                        projectID = Int32.Parse(arg.Remove(0, pidArg.Length));
                    }
                }
            }

            //we don't have any arguments in this demo so just use a random number which is ignored anyway
            projectID = 79;

            //do some other stuff here...

        }
    }
}
