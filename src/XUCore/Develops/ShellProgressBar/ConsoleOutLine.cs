using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.Develops.ShellProgressBar
{
	public struct ConsoleOutLine
	{
		public bool Error { get; }
		public string Line { get; }

		public ConsoleOutLine(string line, bool error = false)
		{
			Error = error;
			Line = line;
		}
	}
}
