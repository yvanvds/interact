using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class Client
  {
    public abstract string IpAddress { get; }
    public abstract string ID { get; }

    // the name used to login
    public abstract string Name { get; }

    // scripts can alter this name for easier reference
    public string LocalName { get; set; }

    public abstract void Invoke(string MethodName, params object[] arguments);

    public abstract void StartScreen(string screenName);
  }
}
