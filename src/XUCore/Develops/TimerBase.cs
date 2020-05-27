using System;
using System.Timers;

namespace XUCore.Develops
{
    public abstract class TimerBase : IDisposable
    {
        public Timer timer = null;

        private bool _alreadyDisposed = false;

        public TimerBase(int Interval)
        {
            timer = new Timer(Interval);
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public abstract void timer_Elapsed(object sender, ElapsedEventArgs e);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                if (this.timer != null)
                {
                    this.timer.Elapsed -= new ElapsedEventHandler(timer_Elapsed);
                    this.timer.Stop();
                    this.timer.Close();
                    this.timer.Dispose();
                }
            }
            _alreadyDisposed = true;
        }
    }
}