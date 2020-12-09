using System.Threading;
using System.Threading.Tasks;

namespace XUCore.ShellProgressBar.Examples
{
	public interface IProgressBarExample
	{
		Task Start(CancellationToken token);
	}
}