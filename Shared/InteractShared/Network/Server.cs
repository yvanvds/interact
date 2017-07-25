using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class Server
  {
    public abstract string IpAddress { get; }
    public abstract string Name { get; }
    public abstract void Log(string Message);
    public abstract void Invoke(string MethodName, params object[] arguments);
  }
}
