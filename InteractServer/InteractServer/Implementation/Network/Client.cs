using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Implementation.Network
{
  public class Client : Interact.Network.Client
  {
    private string ipAddress;
    private string iD;
    private string name;

    public override string IpAddress => ipAddress;
    public override string ID => iD;
    public override string Name => name;

    public Client(string IpAddress, string ID, string Name)
    {
      ipAddress = IpAddress;
      iD = ID;
      name = Name;
    }

    public override void Invoke(string methodName, params object[] arguments)
    {
      Global.Sender.InvokeMethod(ID, methodName, arguments);
    }

    public override void StartScreen(string path)
    {
      Guid screenID = Global.ProjectManager.Current.Screens.Get(path).ID;
      Global.Sender.StartScreen(ID, screenID);
    }
  }
}
