using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/*
 * This class is used for intellisense server scripts,
 * and also to load any added types and objects to
 * server instances of jint.
 */

namespace InteractServer.Intellisense
{
  public class ServerObjects : ScriptObjects
  {
    public ServerObjects()
    {
      //AddScriptType("SensorType", typeof(Interact.SensorType));
      //AddScriptType("Vector", typeof(Interact.Utility.Vector));


      // we want intellisense for these, but not allow to create new objects of this type
      //AddGlobalObject("Log", "LogPage", typeof(Pages.LogPage), Global.Log);
      AddGlobalObject("Root", "GridType", typeof(Implementation.UI.Grid));
      AddGlobalObject("Server", "ServerInstance", typeof(Implementation.Network.Server));
      AddGlobalObject("Clients", "ClientWrapper", typeof(Implementation.Network.Clients));

      // UI
      AddScriptType("Button", typeof(Implementation.UI.Button));
      AddScriptType("Image", typeof(Implementation.UI.Image));
      AddEnum("ImageMode", typeof(Implementation.UI.Image.Mode));
      AddScriptType("Text", typeof(Implementation.UI.Text));
      AddScriptType("Title", typeof(Implementation.UI.Title));
      AddScriptType("Slider", typeof(Implementation.UI.Slider));
      AddScriptType("Grid", typeof(Implementation.UI.Grid));
      AddScriptType("Color", typeof(Implementation.UI.Color));

      AddScriptType("CCanvas", typeof(Implementation.UI.Canvas.CCanvas));
      AddScriptType("CCircle", typeof(Implementation.UI.Canvas.CCircle));
      AddScriptType("CRect", typeof(Implementation.UI.Canvas.CRect));
      AddScriptType("CLine", typeof(Implementation.UI.Canvas.CLine));
      AddScriptType("CLayer", typeof(Implementation.UI.Canvas.CLayer));

      // Network
      AddScriptType("OscSender", typeof(Implementation.Network.OscSender));
      AddScriptType("OscReceiver", typeof(Implementation.Network.OscReceiver));
      AddScriptType("GlMixer", typeof(Implementation.Network.GlMixer));

      // logic
      AddScriptType("Timer", typeof(Implementation.Logic.Timer));
      AddScriptType("Patcher", typeof(Implementation.Logic.Patcher));

      // Utility
      AddScriptType("Coordinate", typeof(Interact.Utility.Coordinate));
      
    }
  }
}
