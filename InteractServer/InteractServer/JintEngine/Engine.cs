using InteractServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Runtime.Interop;
using System.Windows.Controls;
using System.Windows;
using InteractServer.Project;

namespace InteractServer.JintEngine
{
  static public class Runner
  {
    static public Engine Engine = null;

    static public RunningProjectPage pageView = null;

    static public void Start()
    {
      Stop();

      pageView = new RunningProjectPage();
      Engine = new Engine();

      Engine.StartCurrentProject();
      Global.Log.AddEntry("Server Scripts are now running.");
    }

    static public void Stop()
    {
      if (pageView != null)
      {
        pageView.Dispose();
        pageView = null;
      }

      if (Engine != null)
      {
        Engine = null;
        Global.Log.AddEntry("Server Scripts are now stopped.");
      }
    }
  }


  public class Engine
  {
    private Jint.Engine jint = null;

    private Implementation.Network.Clients Clients;
    private Implementation.Network.Server Server;

    public Engine()
    {
      jint = new Jint.Engine(cfg => {
        cfg.AllowClr();
      });
      Global.ServerObjects.InsertInto(jint);
      jint.SetValue("Root", Runner.pageView.PageRoot);

      Clients = new Implementation.Network.Clients();
      jint.SetValue("Clients", Clients);

      Server = new Implementation.Network.Server();
      jint.SetValue("Server", Server);
    }

    public void StartCurrentProject()
    {
      ObservableCollection<IDiskResource> resources = Global.ProjectManager.Current.ServerScripts.Resources;

      foreach (IDiskResource resource in resources)
      {
        try
        {
          jint.Execute((resource as Project.ServerScript.Item).Content);
          Global.Log.AddEntry("Added ServerScript " + (resource as Project.ServerScript.Item).Name);
        }
        catch (Esprima.ParserException e)
        {
          Global.ErrorLog.AddEntry(e.Index, e.LineNumber, e.Message, resource);
        }
        catch (Exception e)
        {
          Global.ErrorLog.AddEntry(0, 0, e.GetType().ToString() +  "ServerScript " + (resource as Project.ServerScript.Item).Name + " error: " + e.ToString());
          return;
        }

      }

      InvokeMethod("Init");
    }

    public void InvokeMethod(string name)
    {
      if (Global.ProjectManager.Current.ServerScripts.Resources.Count == 0) return;

      App.Current.Dispatcher.Invoke((Action)delegate
      {
        try
        {
          jint.Invoke(name);
        }
        catch (Jint.Runtime.JavaScriptException e)
        {
          Global.ErrorLog.AddEntry(0, e.LineNumber, e.Error.ToString());
        }
        catch (Exception e)
        {
          Global.Log.AddEntry("Serverscript error: " + e.ToString());
        }
      });
    }

    public void InvokeMethod(string name, params object[] arguments)
    {
      if (Global.ProjectManager.Current.ServerScripts.Resources.Count == 0) return;

      App.Current.Dispatcher.Invoke((Action)delegate
      {
        try
        {
          jint.Invoke(name, arguments);
        }
        catch (Jint.Runtime.JavaScriptException e)
        {
          Global.ErrorLog.AddEntry(0, e.LineNumber, e.Error.ToString());
        }
        catch (Exception e)
        {
          Global.Log.AddEntry("Serverscript error: " + e.ToString());
        }
      });
    }
  }
}
