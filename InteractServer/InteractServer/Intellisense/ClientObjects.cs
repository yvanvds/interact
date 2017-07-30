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
      AddGlobalObject("Values", "ValuesType", typeof(Interact.Values));
      AddGlobalObject("Project", "ProjectType", typeof(FakeClientClasses.ProjectStorage));
      AddGlobalObject("Server", "ServerType", typeof(Interact.Network.Server));
      AddGlobalObject("Clients", "ClientsType", typeof(Interact.Network.Clients));
      AddGlobalObject("Client", "ClientType", typeof(Interact.Network.Client));

      // Device
      AddGlobalObject("Sensors", "SensorsType", typeof(Interact.Device.Sensors));
      AddScriptType("Sensor", typeof(Interact.Device.Sensor));
      AddGlobalObject("Arduino", "ArduinoType", typeof(Interact.Device.Arduino));
      AddEnum("PinMode", typeof(Interact.Device.Arduino.PinMode));
      AddEnum("PinState", typeof(Interact.Device.Arduino.PinState));

      AddScriptType("Button", typeof(Interact.UI.Button));
      AddScriptType("Entry", typeof(Xamarin.Forms.Entry));
      AddScriptType("Image", typeof(Interact.UI.Image));
      AddScriptType("Text", typeof(Interact.UI.Text));
      AddScriptType("Title", typeof(Interact.UI.Title));
      AddScriptType("Grid", typeof(Interact.UI.Grid));
      AddScriptType("Slider", typeof(Interact.UI.Slider));

      AddScriptType("Color", typeof(Interact.UI.Color));

      AddScriptType("SensorVector", typeof(Interact.Utility.SensorVector));

      AddScriptType("OscSender", typeof(Interact.Network.OscSender));
      AddScriptType("OscReceiver", typeof(Interact.Network.OscReceiver));
    }
  }
}
