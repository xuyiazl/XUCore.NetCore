using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Develops.ShellProgressBar;

namespace XUCore.NetCore.Sample
{
    class Program
    {
        protected static void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action<int> childAction = null)
        {
            var initialMessage = pbar.Message;
            for (var i = 0; i < ticks; i++)
            {
                pbar.Message = $"Start {i + 1} of {ticks} : {initialMessage}";
                childAction?.Invoke(i);
                Thread.Sleep(sleep);
                pbar.Tick($"End {i + 1} of {ticks} : {initialMessage}");
            }
        }
        static void Main(string[] args)
        {
            const int totalTicks = 10;
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressCharacter = '─'
            };
            var childOptions = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                BackgroundColor = ConsoleColor.DarkGray,
                ProgressCharacter = '─',
                CollapseWhenFinished = true
            };
            using (var pbar = new ProgressBar(totalTicks, "main progressbar", options))
            {
                TickToCompletion(pbar, totalTicks, sleep: 100, childAction: i =>
                {
                    using (var child = pbar.Spawn(totalTicks, "child actions", childOptions))
                    {
                        TickToCompletion(child, totalTicks, sleep: 100);
                    }
                });
            }

            //const int totalTicks = 5;
            //using (var pbar = new ProgressBar(totalTicks, "only draw progress on tick"))
            //{
            //    TickToCompletion(pbar, totalTicks, sleep: 1000);
            //}
            Console.WriteLine("press any key to return continue...");
            Console.ReadKey();
        }
    }
}
