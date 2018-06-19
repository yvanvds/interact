using InteractServer.Controls;
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
  public class ScreenView : IView
  {
    public Guid ID => Screen.ID;

    private Project.Screen.Item screen;
    public Project.Screen.Item Screen { get => screen; set => screen = value; }

    private LayoutDocument document;
    public LayoutDocument Document { get => document; set => document = value; }

    public bool Tainted { get => Screen.Tainted; }

    private ContentType type;
    public ContentType Type => type;

    public ScreenView(IContentModel screen)
    {
      Screen = screen as Project.Screen.Item;
      Document = new LayoutDocument();
      type = ContentType.Screen;

      Document.Content = new Frame() { Content = GeneratePageForScreen(Screen) };
      Document.Title = screen.Name;
      Document.Closing += Document_Closing;
    }

    private void Document_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (Screen.Tainted)
      {
        var result = Messages.RequestScreenSave(Screen.Name);

        if (result.Equals(MessageBoxResult.Cancel))
        {
          e.Cancel = true;
        }
        else if (result.Equals(MessageBoxResult.Yes))
        {
          Save();
          e.Cancel = false;
        }
        else if (result.Equals(MessageBoxResult.No))
        {
          DiscardChanges();
          e.Cancel = false;
        }
      }
    }

    public void DetachfromParent()
    {
      Document.Parent?.RemoveChild(Document);
    }


    public void Save()
    {
      if (Screen.Tainted)
      {
        Frame f = Document.Content as Frame;
        BasePage sp = f.Content as BasePage;
        sp.Save();
        Global.ProjectManager.Current.Screens.Save(Screen);
        Screen.Tainted = false;
        Document.Title = Screen.Name;
      }
    }

    public void DiscardChanges()
    {
      if (Screen.Tainted)
      {
        Frame f = Document.Content as Frame;
        BasePage sp = f.Content as BasePage;
        sp.DiscardChanges();
        Screen.Tainted = false;
        Document.Title = Screen.Name;
      }
    }

    public void Taint()
    {
      if (!Screen.Tainted)
      {
        Screen.Tainted = true;
        Document.Title = Screen.Name + " *";
      }
    }

   

    

    public CodeEditor Editor
    {
      get
      {
        Frame f = Document.Content as Frame;
        ScriptPage sp = f.Content as ScriptPage;

        if (sp != null) return sp.GetCodeEditor();
        return null;
      }
    }

    private BasePage GeneratePageForScreen(Project.Screen.Item screen)
    {
      if (screen.ScreenType.Equals(Project.Screen.ScreenTypes.Script)) return new ScriptPage(this);
      if (screen.ScreenType.Equals(Project.Screen.ScreenTypes.UtilityScript)) return new ScriptPage(this);
      return null;
    }




  }
}
