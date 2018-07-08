using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.Network
{
  public class Client : Interact.Network.Client
  {
    private string ipAddress;
    private Guid id;
    private string name;
    private bool local = false;

    public override string IpAddress => ipAddress;

    public override Guid ID => id;

    public override string Name => name;

    public bool Local => local;

    public Client(string ipAddress, Guid id, string name, bool local)
    {
      this.ipAddress = ipAddress;
      this.id = id;
      this.name = name;
      this.local = local;
    }

    public override void Invoke(string MethodName, params object[] arguments)
    {
      if(local)
      {
        JintEngine.Engine.Instance.Invoke(MethodName, arguments);
      } else
      {
        InteractClient.Network.Sender.Get().InvokeMethod(ID, MethodName, arguments);
      }
    }

    public override void StartScreen(string screenName)
    {
      Data.Screen screen = Data.Project.Current.GetScreen(screenName);
      if(screen == null)
      {
        InteractClient.Network.Sender.Get().WriteLog("Error: the screen to start does not exist.");
        return;
      }

      if(local)
      {
        JintEngine.Engine.Instance.StartScreen(screen.ID);
      } else
      {
        InteractClient.Network.Sender.Get().StartScreen(ID, screen.ID);
      }
    }
  }
}
