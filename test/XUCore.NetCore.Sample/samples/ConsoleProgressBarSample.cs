using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using XUCore.Develops;

namespace XUCore.NetCore.Sample.samples
{
    public class ConsoleProgressBarSample : ISample
    {
        public void Run()
        {
            ConsoleProgressBar bar1 = new ConsoleProgressBar("装逼 Win2003", ConsoleProgressBarTheme.Win2003);

            for (var ndx = 1; ndx <= 100; ndx++)
            {
                bar1.Update(ndx, 100, $"我是进度啊：{ndx}");

                for (var ndx1 = 1; ndx1 <= 10; ndx1++)
                {
                    Console.WriteLine($"aaaaaaaaaaaa{ndx1}");
                }
                Thread.Sleep(100);
            }
            ConsoleProgressBar bar2 = new ConsoleProgressBar("装逼 WinXP", ConsoleProgressBarTheme.WinXP);

            for (var ndx = 1; ndx <= 100; ndx++)
            {
                bar2.Update(ndx, 100, $"我是进度啊：{ndx}");

                Thread.Sleep(100);
            }
            ConsoleProgressBar bar3 = new ConsoleProgressBar("装逼 Win8", ConsoleProgressBarTheme.Win8);

            for (var ndx = 1; ndx <= 100; ndx++)
            {
                bar3.Update(ndx, 100, $"我是进度啊：{ndx}");

                Thread.Sleep(100);
            }
        }
    }
}
