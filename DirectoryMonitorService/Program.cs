using System;
using System.Collections.Generic;

//using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryMonitorService
{
    internal static class Program
    {
        private static void Main()
        {
            //YOu can remove the if debug I just used this to debug the code instead  of setting up the service
#if DEBUG
            MonitorService myService = new MonitorService();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MonitorService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}