using InteractServer.Controls;
using InteractServer.Dialogs;
using InteractServer.Models;
using InteractServer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Managers
{
  public class ServerScriptManager
  {
    Collection<ServerScriptView> List;

    public ServerScriptManager()
    {
      List = new Collection<ServerScriptView>();
    }

    public void StartNewServerScript()
    {
      NewServerScriptDialog dlg = new NewServerScriptDialog();
      dlg.ShowDialog();
    }

    public void CreateNewServerScript(String scriptName)
    {
      if(Global.ProjectManager.Current == null)
      {
        Messages.NoOpenProject();
        return;
      }

      ServerScript script = Global.ProjectManager.Current.ServerScripts.CreateServerScript(scriptName);
      AddAndShow(script);
    }

    public void AddAndShow(ServerScript script)
    {
      foreach (ServerScriptView view in List)
      {
        if (view.ID == script.ID)
        {
          // document is already open, just give it focus
          Global.AppWindow.AddDocument(view.Document);
          return;
        }
      }

      // if we get here, a new documentwindow must be added
      ServerScriptView newView = new ServerScriptView(script);
      List.Add(newView);
      Global.AppWindow.AddDocument(newView.Document);

    }

    public CodeEditor GetCodeEditor(ServerScript script)
    {
      foreach(ServerScriptView view in List)
      {
        if(view.ID == script.ID)
        {
          return view.CodeEditor;
        }
      }
      return null;
    }

    public void Close(ServerScript script)
    {
      foreach (ServerScriptView view in List)
      {
        if (view.ID == script.ID)
        {
          Global.AppWindow.CloseDocument(view.Document);
          List.Remove(view);
          return;
        }
      }
    }

    public void RefreshName(ServerScript script)
    {
      foreach (ServerScriptView view in List)
      {
        if (view.ID == script.ID)
        {
          view.Document.Title = script.Name;
          return;
        }
      }
    }

    public void SaveAll()
    {
      foreach (ServerScriptView view in List)
      {
        view.Save();
      }
    }

    public bool NeedsSaving()
    {
      foreach (ServerScriptView view in List)
      {
        if (view.Tainted()) return true;
      }
      return false;
    }

    public void Clear()
    {
      foreach (ServerScriptView view in List)
      {
        view.DetachfromParent();
      }
      List.Clear();
    }
  }
}
