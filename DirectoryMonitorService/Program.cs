
using System;
using System.Collections.Generic;
//using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace DirectoryMonitorService
{
    static class Program
    {

        static void Main()
        {


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MonitorService()
                
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
