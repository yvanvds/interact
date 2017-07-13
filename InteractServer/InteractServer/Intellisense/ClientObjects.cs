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
      AddGlobalObject("Client", "ClientType", typeof(Xamarin.Forms.ContentPage));
      AddGlobalObject("Root", "ClientRootType", typeof(Interact.UI.Grid));
      AddGlobalObject("Project", "ProjectType", typeof(FakeClientClasses.ProjectStorage));
      AddGlobalObject("Server", "ServerType", typeof(FakeClientClasses.Server));

      AddScriptType("Button", typeof(Interact.UI.Button));
      AddScriptType("Entry", typeof(Xamarin.Forms.Entry));
      AddScriptType("Image", typeof(Interact.UI.Image));
      AddScriptType("Text", typeof(Interact.UI.Text));
      AddScriptType("Title", typeof(Interact.UI.Title));
      AddScriptType("Grid", typeof(Interact.UI.Grid));
      AddScriptType("StackPanel", typeof(Xamarin.Forms.StackLayout));

      AddScriptType("ColumnDefinition", typeof(Xamarin.Forms.ColumnDefinition));
      AddScriptType("RowDefinition", typeof(Xamarin.Forms.RowDefinition));
      AddScriptType("GridLength", typeof(Xamarin.Forms.GridLength));
      AddScriptType("GridUnitType", typeof(Xamarin.Forms.GridUnitType));
      AddScriptType("Orientation", typeof(Xamarin.Forms.StackOrientation));
      AddScriptType("Thickness", typeof(Xamarin.Forms.Thickness));
      AddScriptType("TextAlignment", typeof(Xamarin.Forms.TextAlignment));

      AddScriptType("Color", typeof(Interact.UI.Color));
    }
  }
}
