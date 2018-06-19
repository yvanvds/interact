using InteractServer.Controls;
using InteractServer.Dialogs;
using InteractServer.Models;
using InteractServer.Project;
using InteractServer.Project.Patcher;
using InteractServer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Managers
{
  public class ViewManager
  {
    Collection<IView> List;

    public ViewManager()
    {
      List = new Collection<IView>();
    }

    public void StartNewView(ContentType type)
    {
      IDialog dlg = null;
      switch(type)
      {
        case ContentType.Screen:
            dlg = new NewScreenDialog();
            break;
          
        case ContentType.ServerScript:
          dlg = new NewServerScriptDialog();
          break;

        case ContentType.Patcher:
          dlg = new NewPatcherDialog();
          break;
      }
      bool? result = dlg.ShowDialog();
      if(result.HasValue)
      {
        if((bool)result)
        {
          if(Global.ProjectManager.Current == null)
          {
            Messages.NoOpenProject();
            return;
          }

          IContentModel model = null;

          switch (type)
          {
            case ContentType.Screen:
              model = Global.ProjectManager.Current.Screens.Create(dlg.ModelName, dlg.Type);
              break;

            case ContentType.ServerScript:
              model = Global.ProjectManager.Current.ServerScripts.Create(dlg.ModelName);
              break;

            case ContentType.Patcher:
              model = Global.ProjectManager.Current.Patchers.Create(dlg.ModelName);
              break;
          }

          if(model != null)
          {
            AddAndShow(model);
          }
        }
      }
    }

    public void AddAndShow(IContentModel model)
    {
      foreach(IView view in List)
      {
        if(view.ID == model.ID && view.Type == model.Type)
        {
          Global.AppWindow.AddDocument(view.Document);
          return;
        }
      }

      IView newView = null;
      switch(model.Type)
      {
        case ContentType.Screen:
          newView = new ScreenView(model);
          break;
        case ContentType.ServerScript:
          newView = new ServerScriptView(model);
          break;
        case ContentType.Patcher:
          Global.AudioManager.AddObject(model as Item);
          newView = new PatcherView(model);
          break;
      }

      if(newView != null)
      {
        List.Add(newView);
        Global.AppWindow.AddDocument(newView.Document);
      }
    }

    public void Close(IContentModel model)
    {
      foreach(IView view in List)
      {
        if(view.ID == model.ID && view.Type == model.Type)
        {
          //Global.AppWindow.CloseDocument(view.Document);
          List.Remove(view);
          
          return;
        }
      }
    }

    public void RefreshName(IContentModel model)
    {
      foreach(IView view in List)
      {
        if(view.ID == model.ID && view.Type == model.Type)
        {
          view.Document.Title = model.Name;
          return;
        }
      }
    }

    public void SaveAll()
    {
      foreach(IView view in List)
      {
        view.Save();
      }
    }

    public bool NeedsSaving()
    {
      foreach (IView view in List)
      {
        if (view.Tainted) return true;
      }
      return false;
    }

    public void Clear()
    {
      foreach(IView view in List)
      {
        Global.AppWindow.CloseDocument(view.Document);
      }
      List.Clear();
    }

    public CodeEditor GetCodeEditor(IContentModel model)
    {
      foreach(IView view in List)
      {
        if(view.ID == model.ID && view.Type == model.Type)
        {
          return view.Editor;
        }
      }
      return null;
    }
  }
}
