using System;

namespace XUCore.NetCore.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ISample sample = new FlowMonitoringSample();

            sample.Run();

            Console.WriteLine("press any key to return continue...");
            Console.ReadKey();
        }
    }
}
