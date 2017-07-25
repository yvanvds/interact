using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class OscReceiver
  {
    public abstract void Init(int port);

    public abstract void Register(string address, string callbackFunction);

    public abstract void Start();
    public abstract void Stop();
  }
}
