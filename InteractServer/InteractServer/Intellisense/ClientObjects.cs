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
      AddGlobalObject("Project", "ProjectType", typeof(FakeClientClasses.ProjectStorage));
      AddGlobalObject("Server", "ServerType", typeof(FakeClientClasses.Server));

      AddScriptType("Button", typeof(Xamarin.Forms.Button));
      AddScriptType("Entry", typeof(Xamarin.Forms.Entry));
      AddScriptType("Image", typeof(Xamarin.Forms.Image));
      AddScriptType("Text", typeof(Xamarin.Forms.Label));
      AddScriptType("Title", typeof(Xamarin.Forms.Label));
    }
  }
}
