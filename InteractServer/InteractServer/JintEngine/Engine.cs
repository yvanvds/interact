using InteractServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Runtime.Interop;


namespace InteractServer.JintEngine
{
  static public class Runner
  {
    static public Engine Engine = null;

    static public void Start()
    {
      Stop();

      Engine = new Engine();
      Engine.StartCurrentProject();

      Global.Log.AddEntry("Server Scripts are now running.");
    }

    static public void Stop()
    {
      if(Engine != null)
      {
        Engine = null;
        Global.Log.AddEntry("Server Scripts are now stopped.");
      }
    }
  }


  public class Engine
  {
    private Jint.Engine jint = null;

    public Engine()
    {
      jint = new Jint.Engine(cfg => cfg.AllowClr());
      jint.SetValue("Log", Global.Log);
      jint.SetValue("Osc", TypeReference.CreateTypeReference(jint, typeof(Osc)));
    }

    public void StartCurrentProject()
    {
      ObservableCollection<ProjectResource> resources = Global.ProjectManager.Current.ServerScripts.Resources;

      foreach(ProjectResource resource in resources)
      {
        try
        {
          jint.Execute((resource as ServerScript).Content);
          Global.Log.AddEntry("Added ServerScript " + (resource as ServerScript).Name);
        } catch(Exception e)
        {
          Global.Log.AddEntry("ServerScript " + (resource as ServerScript).Name + " error: " + e.ToString());
          return;
        }
        
      }

      InvokeMethod("Init");

      
    }

    public void InvokeMethod(string name)
    {
      try
      {
        jint.Invoke(name);
      }
      catch (Exception e)
      {
        Global.Log.AddEntry("Serverscript error: " + e.ToString());
      }
    }

    public void InvokeMethod(string name, params object[] arguments)
    {
      try
      {
        jint.Invoke(name, arguments);
      }
      catch (Exception e)
      {
        Global.Log.AddEntry("Serverscript error: " + e.ToString());
      }
    }
  }
}
