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
    private string iD;
    private string name;
    private bool local = false;

    public override string IpAddress => ipAddress;

    public override string ID => iD;

    public override string Name => name;

    public bool Local => local;

    public Client(string IpAddress, string ID, string Name, bool Local)
    {
      ipAddress = IpAddress;
      iD = ID;
      name = Name;
      local = Local;
    }

    public override void Invoke(string MethodName, params object[] arguments)
    {
      if(local)
      {
        JintEngine.Engine.Instance.Invoke(MethodName, arguments);
      } else
      {
        InteractClient.Network.Service.Get().InvokeMethod(ID, MethodName, arguments);
      }
    }

    public override void StartScreen(string screenName)
    {
      if(local)
      {
        JintEngine.Engine.Instance.StartScreen(Data.Project.Current.GetScreen(screenName).ID);
      } else
      {
        InteractClient.Network.Service.Get().StartScreen(ID, Data.Project.Current.GetScreen(screenName).ID);
      }
    }
  }
}
