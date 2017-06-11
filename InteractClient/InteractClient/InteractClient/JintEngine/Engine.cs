using InteractClient.Data;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.JintEngine
{
  class Engine
  {
    public static Engine Instance = new Engine();
    public static UI.Eventhandler EventHandler = new UI.Eventhandler();

    private ContentPage activePage;

    private bool ScreenNeedsToStart = false;
    private int screenToStart;

    // Jint Engine
    private Jint.Engine jEngine = null;
    private ProjectStorage Storage = new ProjectStorage();
    private Network.Server Server = new Network.Server(); 

    public Jint.Engine Jint()
    {
      if (jEngine == null)
      {
        jEngine = new Jint.Engine();
        jEngine.SetValue("Client", activePage as ModelPage);
        jEngine.SetValue("Project", Storage);

        // UI
        jEngine.SetValue("Button", TypeReference.CreateTypeReference(jEngine, typeof(UI.Button)));
        jEngine.SetValue("Title", TypeReference.CreateTypeReference(jEngine, typeof(UI.Title)));
        jEngine.SetValue("Text", TypeReference.CreateTypeReference(jEngine, typeof(UI.Text)));
        jEngine.SetValue("Image", TypeReference.CreateTypeReference(jEngine, typeof(UI.Image)));
        jEngine.SetValue("Entry", TypeReference.CreateTypeReference(jEngine, typeof(UI.Entry)));


        // Network
        jEngine.SetValue("Server", Server);
      }
      return jEngine;
    }

    public void StartScreen(int ID)
    {
      if (activePage is ConnectedPage)
      {
        ScreenNeedsToStart = true;
        screenToStart = ID;
        ConnectedPage page = activePage as ConnectedPage;
        Device.BeginInvokeOnMainThread(() => page.PushModelPage());
        InteractClient.Network.Service.Get().GetNextMethod();

      }
      else if (activePage is ModelPage)
      {
        ModelPage p = activePage as ModelPage;
        Device.BeginInvokeOnMainThread(() => StartScript(ID));
      }
    }

    public void StopScreen()
    {
      if (activePage is ModelPage)
      {
        ModelPage p = activePage as ModelPage;
        Device.BeginInvokeOnMainThread(() => p.Pop());
        jEngine = null;
        EventHandler.Clear();
      }
      InteractClient.Network.Service.Get().GetNextMethod();
    }

    public void SetActivePage(ContentPage page)
    {
      activePage = page;
      if (page is ModelPage && ScreenNeedsToStart)
      {
        ScreenNeedsToStart = false;
        StartScript(screenToStart);
      }
    }

    public void StartScript(int ID)
    {
      Screen screen = Project.Current.GetScreen(ID);

      if (screen == null)
      {
        InteractClient.Network.Service.Get().WriteLog("Engine->StartScript: screen " + ID + " not found.");
        return;
      }
      try
      {

        Jint().Execute(screen.screenContent.Content);
        Jint().Invoke("Init");
        InteractClient.Network.Service.Get().WriteLog("Engine->StartScript: screen " + ID + " is now running.");
      }
      catch (Jint.Parser.ParserException e)
      {
        InteractClient.Network.Service.Get().WriteLog("Engine->StartScript: Parse Eror on screen " + ID + ":" + e.Message);
      }
      catch (Jint.Runtime.JavaScriptException e)
      {
        InteractClient.Network.Service.Get().WriteLog("Engine->StartScript: Runtime Eror on screen " + ID + ":" + e.Message);
      }
      catch (Exception e)
      {
        InteractClient.Network.Service.Get().WriteLog("Engine->StartScript: Eror on screen " + ID + ":" + e.Message);
      }
    }

    public void Invoke(String functionName, params object[] arguments)
    {
      Jint().Invoke(functionName, arguments);
    }

    public void SetScreenMessage(string message, bool animate)
    {
      if (activePage == null) return;
      if (activePage is ModelPage) return;
      Device.BeginInvokeOnMainThread(() =>
      {
        (activePage as ConnectedPage)?.SetActivity(message, animate);
      });
    }
  }
}
