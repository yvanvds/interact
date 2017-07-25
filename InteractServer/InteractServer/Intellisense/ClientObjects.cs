using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Intellisense
{
  public class ClientObjects : ScriptObjects
  {
    public ClientObjects()
    {    
      AddGlobalObject("Root", "ClientRootType", typeof(Interact.UI.Grid));
      AddGlobalObject("Project", "ProjectType", typeof(FakeClientClasses.ProjectStorage));
      AddGlobalObject("Server", "ServerType", typeof(Interact.Network.Server));
      AddGlobalObject("Clients", "ClientsType", typeof(Interact.Network.Clients));
      AddGlobalObject("Client", "ClientType", typeof(Interact.Network.Client));

      // Device
      AddGlobalObject("Sensors", "SensorsType", typeof(Interact.Device.Sensors));
      AddScriptType("Sensor", typeof(Interact.Device.Sensor));

      AddScriptType("Button", typeof(Interact.UI.Button));
      AddScriptType("Entry", typeof(Xamarin.Forms.Entry));
      AddScriptType("Image", typeof(Interact.UI.Image));
      AddScriptType("Text", typeof(Interact.UI.Text));
      AddScriptType("Title", typeof(Interact.UI.Title));
      AddScriptType("Grid", typeof(Interact.UI.Grid));

      AddScriptType("Color", typeof(Interact.UI.Color));

      AddScriptType("SensorVector", typeof(Interact.Utility.SensorVector));
    }
  }
}
