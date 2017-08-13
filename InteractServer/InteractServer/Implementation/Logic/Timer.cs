using InteractServer.JintEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace InteractServer.Implementation.Logic
{
  public class Timer : Interact.Logic.Timer
  {
    private DispatcherTimer timer;
    private string callback;

    public override void Start(string callback, int intervalMilliSec)
    {
      this.callback = callback;
      timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromMilliseconds(intervalMilliSec);
      timer.Start();
    }

    public override void Stop()
    {
      timer.Stop();
    }

    private void tick(object sender, EventArgs e)
    {
      if (Runner.Engine != null) Runner.Engine.InvokeMethod(callback);
      else timer.Tick -= tick;
      timer.Stop();
    }
  }
}
