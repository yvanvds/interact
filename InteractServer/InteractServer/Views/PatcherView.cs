using InteractServer.Models;
using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Views
{
  public class PatcherView
  {
    
    private LayoutDocument document;
    public LayoutDocument Document { get => document; set => document = value; }

    public int ID => patcher.ID;

    private Patcher patcher;
    public Patcher Patcher { get => patcher; set => patcher = value; }

    public PatcherView(Patcher patcher)
    {
      this.patcher = patcher;
      document = new LayoutDocument();
      document.Content = new Frame()
      {
        Content = GeneratePageForPatcher(patcher)
      };
      document.Title = patcher.Name;
      document.Closing += Document_Closing;
    }

    private void Document_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if(patcher.Tainted)
      {
        var result = Messages.RequestPatcherSave(patcher.Name);

        if(result.Equals(MessageBoxResult.Cancel))
        {
          e.Cancel = true;
        } else if (result.Equals(MessageBoxResult.Yes))
        {
          Save();
          Frame f = document.Content as Frame;
          PatcherPage sp = f.Content as PatcherPage;
          sp.Close();
          //Global.PatcherManager.Close(patcher);
          e.Cancel = false;
        } else if (result.Equals(MessageBoxResult.No))
        {
          DiscardChanges();
          Frame f = document.Content as Frame;
          PatcherPage sp = f.Content as PatcherPage;
          sp.Close();
          //Global.PatcherManager.Close(patcher);
          e.Cancel = false;
        }
      } else
      {
        Frame f = document.Content as Frame;
        PatcherPage sp = f.Content as PatcherPage;
        sp.Close();
      }
    }

    public void DetachfromParent()
    {
      document.Parent?.RemoveChild(document);
    }

    public void Load()
    {
      Frame f = document.Content as Frame;
      PatcherPage sp = f.Content as PatcherPage;
      sp.Load();
    }

    public void Save()
    {
      if(patcher.Tainted)
      {
        Frame f = document.Content as Frame;
        PatcherPage sp = f.Content as PatcherPage;
        sp.Save();
        Global.ProjectManager.Current.Patchers.Save(Patcher);
        patcher.Tainted = false;
        document.Title = patcher.Name;
      }
    }


    public void DiscardChanges()
    {
      if (Patcher.Tainted)
      {
        Frame f = document.Content as Frame;
        PatcherPage sp = f.Content as PatcherPage;
        sp.DiscardChanges();
        patcher.Tainted = false;
        document.Title = patcher.Name;
      }
    }

    public void Taint()
    {
      if(!patcher.Tainted)
      {
        patcher.Tainted = true;
        document.Title = patcher.Name + " *";
      }
    }

    public bool Tainted()
    {
      return patcher.Tainted;
    }

    private PatcherPage GeneratePageForPatcher(Patcher patcher)
    {
      return new PatcherPage(this);
    }
  }
}
