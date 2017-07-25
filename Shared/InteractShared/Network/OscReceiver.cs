using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class OscReceiver
  {
    public abstract void Register(string address, string callbackFunction);

    public abstract void Start(int port);
    public abstract void Stop();
  }
}
