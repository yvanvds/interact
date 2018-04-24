using InteractServer.Dialogs;
using InteractServer.Models;
using InteractServer.Pages;
using InteractServer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Managers
{
  public class PatcherManager
  {
    Collection<PatcherView> List;

    public PatcherManager()
    {
      List = new Collection<PatcherView>();
    }

    public void StartNewPatcher()
    {
      NewPatcherDialog dlg = new NewPatcherDialog();
      dlg.ShowDialog();
    }

    public void CreateNewPatcher(String patcherName)
    {
      if(Global.ProjectManager == null)
      {
        Messages.NoOpenProject();
        return;
      }

      Patcher patcher = Global.ProjectManager.Current.Patchers.CreatePatcher(patcherName);
      AddAndShow(patcher);
    }

    public void AddAndShow(Patcher patcher)
    {
      foreach (PatcherView view in List)
      {
        if(view.ID == patcher.ID)
        {
          view.Load();
          Global.AppWindow.AddDocument(view.Document);
          return;
        }
      }

      PatcherView newView = new PatcherView(patcher);
      List.Add(newView);
      //newView.Load();
      Global.AppWindow.AddDocument(newView.Document);
    }

    public void Close(Patcher patcher)
    {
      foreach(PatcherView view in List)
      {
        if(view.ID == patcher.ID)
        {
          Frame f = view.Document.Content as Frame;
          PatcherPage sp = f.Content as PatcherPage;
          sp.Close();
          
          Global.AppWindow.CloseDocument(view.Document);
          List.Remove(view);
          return;
        }
      }
    }

    public void RefreshName(Patcher patcher)
    {
      foreach (PatcherView view in List)
      {
        if (view.ID == patcher.ID)
        {
          view.Document.Title = patcher.Name;
          return;
        }
      }
    }

    public void SaveAll()
    {
      foreach(PatcherView view in List)
      {
        view.Save();
      }
    }

    public bool NeedsSaving()
    {
      foreach (PatcherView view in List)
      {
        if (view.Tainted()) return true;
      }
      return false;
    }

    public void Clear()
    {
      foreach(PatcherView view in List)
      {
        view.DetachfromParent();
      }
      List.Clear();
    }
  }
}
