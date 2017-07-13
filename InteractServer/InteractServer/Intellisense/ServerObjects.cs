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
      // we want intellisense for these, but not allow to create new objects of this type
      AddGlobalObject("Log", "LogPage", typeof(Pages.LogPage), Global.Log);
      AddGlobalObject("Root", "GridType", typeof(Implementation.UI.Grid));
      AddGlobalObject("Clients", "ClientWrapper", typeof(JintEngine.Clients), Global.NetworkService.ClientWrapper);

      // UI
      AddScriptType("Button", typeof(Implementation.UI.Button));
      AddScriptType("Text", typeof(Implementation.UI.Text));
      AddScriptType("Title", typeof(Implementation.UI.Title));
      AddScriptType("Grid", typeof(Implementation.UI.Grid));
      AddScriptType("Color", typeof(Implementation.UI.Color));
      //AddScriptType("StackPanel", typeof(StackPanel));

      // UI Modifiers
      AddScriptType("ColumnDefinition", typeof(ColumnDefinition));
      AddScriptType("RowDefinition", typeof(RowDefinition));
      AddScriptType("GridLength", typeof(GridLength));
      AddScriptType("GridUnitType", typeof(GridUnitType));
      AddScriptType("Orientation", typeof(Orientation));
      AddScriptType("Thickness", typeof(Thickness));

      AddScriptType("Osc", typeof(JintEngine.Osc));
      AddScriptType("Timer", typeof(JintEngine.DispatcherTimer));
      AddScriptType("TimeSpan", typeof(TimeSpan));
    }
  }
}
