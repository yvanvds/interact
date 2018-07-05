using System;
using System.Collections.Generic;
using System.Text;


namespace InteractClient.Implementation.Logic
{
  public class Timer : Interact.Logic.Timer
  {
    public bool active = false;

    public override void Start(string callback, int intervalMilliSec)
    {
      active = true;
      Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(intervalMilliSec), () =>
      {
        if (JintEngine.Engine.Instance != null && JintEngine.Engine.Instance.Active)
        {
          JintEngine.Engine.Instance.Invoke(callback);
          return active;
        }
        return false;
      });
    }

    public override void Stop()
    {
      active = false;
    }
  }
}
