﻿using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace XUCore.Develops.ShellProgressBar
{
	public class FixedDurationBar : ProgressBar
	{
		public bool IsCompleted { get; private set; }

		private readonly ManualResetEvent _completedHandle = new ManualResetEvent(false);
		public WaitHandle CompletedHandle => _completedHandle;

		public FixedDurationBar(TimeSpan duration, string message, ConsoleColor color) : this(duration, message, new ProgressBarOptions { ForegroundColor = color }) { }

		public FixedDurationBar(TimeSpan duration, string message, ProgressBarOptions options = null) : base((int)Math.Ceiling(duration.TotalSeconds), message, options)
		{
			if (!this.Options.DisplayTimeInRealTime)
				throw new ArgumentException(
					$"{nameof(ProgressBarOptions)}.{nameof(ProgressBarOptions.DisplayTimeInRealTime)} has to be true for {nameof(FixedDurationBar)}", nameof(options)
				);
		}

		private long _seenTicks = 0;
		protected override void OnTimerTick()
		{
			Interlocked.Increment(ref _seenTicks);
			if (_seenTicks % 2 == 0) this.Tick();
			base.OnTimerTick();
		}

		protected override void OnDone()
		{
			this.IsCompleted = true;
			this._completedHandle.Set();
		}
	}
}
