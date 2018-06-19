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
  public class ServerScriptView : IView
  {
    private Project.ServerScript.Item script;
    public Project.ServerScript.Item ServerScript { get => script; set => script = value; }

    private LayoutDocument document;
    public LayoutDocument Document { get => document; set => document = value; }

    public bool Tainted => script.Tainted;
    public Guid ID => script.ID;

    private ContentType type;
    public ContentType Type => type;

    public ServerScriptView(IContentModel model)
    {
      this.script = model as Project.ServerScript.Item;
      Document = new LayoutDocument();
      type = ContentType.ServerScript;

      Document.Content = new Frame() { Content = GeneratePageForScript(script) };
      Document.Title = script.Name;
      Document.Closing += Document_Closing;
    }

    private void Document_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (script.Tainted)
      {
        var result = Messages.RequestScriptSave(script.Name);

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
      Document.Parent.RemoveChild(Document);
    }


    public void Save()
    {
      if (script.Tainted)
      {
        Frame f = Document.Content as Frame;
        BasePage sp = f.Content as BasePage;
        sp.Save();
        Global.ProjectManager.Current.ServerScripts.Save(script);
        script.Tainted = false;
        Document.Title = script.Name;
      }
    }

    public void DiscardChanges()
    {
      if (script.Tainted)
      {
        Frame f = Document.Content as Frame;
        BasePage sp = f.Content as BasePage;
        sp.DiscardChanges();
        script.Tainted = false;
        Document.Title = script.Name;
      }
    }

    public void Taint()
    {
      if (!script.Tainted)
      {
        script.Tainted = true;
        Document.Title = script.Name + " *";
      }
    }

    

    
    public CodeEditor Editor
    {
      get
      {
        Frame f = Document.Content as Frame;
        ScriptPage sp = f.Content as ScriptPage;

        if(sp != null) return sp.GetCodeEditor();
        return null;
      }
    }

    private BasePage GeneratePageForScript(Project.ServerScript.Item script)
    {
      return new ScriptPage(this);
    }

    


  }
}

