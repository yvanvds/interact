using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class OscSender
  {
    public bool AllowDouble { get; set; } = true;

    public abstract void Init(int port);
    public abstract void Init(string address, int port);

    public abstract void Send(string address, params object[] arguments);
  }
}
