using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class Clients
  {
    public abstract Client this[int key] { get; }
    public abstract int Count { get; }

    public abstract Client GetLocal();

    public abstract void Invoke(string MethodName, params object[] arguments);

    public abstract void StartScreen(string screenName);
  }
}
