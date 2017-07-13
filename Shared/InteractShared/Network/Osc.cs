using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class Osc
  {
    protected Rug.Osc.OscSender sender;
    public string BaseAddress { get; set; }

    public abstract void Init(int port);
    public abstract void Init(string address, int port);

    public abstract void Send(string address, params object[] arguments);
  }
}
