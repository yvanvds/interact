using InteractServer.Models;
using InteractServer.Pages;
using InteractServer.Project;
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
  public class PatcherView: IView
  {

    private LayoutDocument document;
    public LayoutDocument Document { get => document; set => document = value; }

    public Guid ID => patcher.ID;

    private Project.Patcher.Item patcher;
    public Project.Patcher.Item Patcher { get => patcher; set => patcher = value; }

    public bool Tainted { get => patcher.Tainted; }

    private ContentType type;
    public ContentType Type => type;

    public Controls.CodeEditor Editor { get => null; }

    public PatcherView(IContentModel model)
    {
      this.patcher = model as Project.Patcher.Item;
      type = ContentType.Patcher;

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
      if (patcher.Tainted)
      {
        var result = Messages.RequestPatcherSave(patcher.Name);

        if (result.Equals(MessageBoxResult.Cancel))
        {
          e.Cancel = true;
        }
        else if (result.Equals(MessageBoxResult.Yes))
        {
          Save();
          PatcherPage sp = GetPage();
          sp.Clear();
          Global.AudioManager.DeleteObject(patcher);
          Global.ViewManager.Close(patcher);
          e.Cancel = false;
        }
        else if (result.Equals(MessageBoxResult.No))
        {
          PatcherPage sp = GetPage();
          sp.Clear();
          Global.AudioManager.DeleteObject(patcher);
          Global.ViewManager.Close(patcher);
          e.Cancel = false;
        }
      }
    }

    public void DetachfromParent()
    {
      document.Parent?.RemoveChild(document);
    }

    public void Save()
    {
      if (patcher.Tainted)
      {
        PatcherPage sp = GetPage();
        sp.Save();
        Global.ProjectManager.Current.Patchers.Save(Patcher);
        patcher.Tainted = false;
        document.Title = patcher.Name;
      }
    }

    public PatcherPage GetPage()
    {
      Frame f = document.Content as Frame;
      return f.Content as PatcherPage;
    }


    public void DiscardChanges()
    {
      if (Patcher.Tainted)
      {
        PatcherPage sp = GetPage();
        sp.DiscardChanges();
        patcher.Tainted = false;
        document.Title = patcher.Name;
      }
    }

    public void Taint()
    {
      if (!patcher.Tainted)
      {
        patcher.Tainted = true;
        document.Title = patcher.Name + " *";
      }
    }

    

    private PatcherPage GeneratePageForPatcher(Project.Patcher.Item patcher)
    {
      return new PatcherPage(this);
    }
  }
}
