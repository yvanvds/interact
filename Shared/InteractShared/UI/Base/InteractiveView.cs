using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class InteractiveView : View
  {
    public abstract void SendOSC(string destination, int port, string address);
    public abstract void SendToPatcher(Interact.Logic.Patcher patcher, string inlet);
  }
}
