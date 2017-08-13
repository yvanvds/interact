using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Logic
{
  public abstract class Timer
  {
    public abstract void Start(string callback, int intervalMilliSec);
    public abstract void Stop();
  }
}
