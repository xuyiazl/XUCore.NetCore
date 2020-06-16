using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Develops;
using XUCore.Extensions;

namespace XUCore.NetCore.Sample
{
    public class FlowMonitoringSample : ISample
    {
        public void Run()
        {
            var flow = new FlowMonitoring();
            flow.Size = 1024;
            flow.Monitoring(kb =>
            {
                Console.WriteLine($"每秒写入数据大小：{kb.Rounding()} kb");
            });

            var users = SampleData.GetUsers(1000000);

            flow.Control(users, (items) =>
            {
                Console.WriteLine(items.Length);
            });
        }
    }
}