using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.JintEngine
{
  class DispatcherTimer : System.Windows.Threading.DispatcherTimer
  {
    private string func;

    public void OnTick (string function)
    {
      func = function;
      Tick += new EventHandler(Timer_Tick);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      if (Runner.Engine != null) Runner.Engine.InvokeMethod(func);
      else
      {
        Tick -= Timer_Tick;
        Stop();
      }
    }

    ~DispatcherTimer()
    {
      Global.Log.AddEntry("timer deleted");
    }
  }
}
