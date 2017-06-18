using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Intellisense.FakeClientClasses
{
  public class Server
  {
    public void InvokeMethod(string name, params object[] arguments) { }
    public void Log(string name) { }
  }
}
