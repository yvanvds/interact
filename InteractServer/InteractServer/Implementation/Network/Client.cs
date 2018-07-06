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
    private Guid id;
    private string name;

    public override string IpAddress
		{
			get => ipAddress;
			
		}

    public override Guid ID => id;
    public override string Name => name;

    public Client(string ipAddress, Guid id, string name)
    {
      this.ipAddress = ipAddress;
      this.id = id;
      this.name = name;
    }

    public override void Invoke(string methodName, params object[] arguments)
    {
			Global.Clients.Get(id).Send.Invoke(methodName, arguments);
    }

    public override void StartScreen(string path)
    {
      Guid screenID = Global.ProjectManager.Current.Screens.Get(path).ID;
			Global.Clients.Get(id).Send.ScreenStart(screenID);
    }
  }
}
