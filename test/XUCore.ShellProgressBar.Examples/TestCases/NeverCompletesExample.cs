using System.Threading;
using System.Threading.Tasks;
using XUCore.Develops.ShellProgressBar;

namespace XUCore.ShellProgressBar.Examples.TestCases
{
	public class NeverCompletesExample : IProgressBarExample
	{
		public Task Start(CancellationToken token)
		{
			var ticks = 5;
			using (var pbar = new ProgressBar(ticks, "A console progress bar does not complete"))
			{
				pbar.Tick();
				pbar.Tick();
				pbar.Tick();
				pbar.Tick();
			}
			return Task.FromResult(1);
		}
	}
}