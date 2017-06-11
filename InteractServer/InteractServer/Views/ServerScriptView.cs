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
  public class ServerScriptView
  {
    public ServerScriptView(ServerScript script)
    {
      this.script = script;
      Document = new LayoutDocument();
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

    public bool Tainted()
    {
      return script.Tainted;
    }

    public int ID => script.ID;

    public ServerScript ServerScript { get => script; set => script = value; }
    public LayoutDocument Document { get => document; set => document = value; }

    private BasePage GeneratePageForScript(ServerScript script)
    {
      return new ScriptPage(this);
    }

    private ServerScript script;
    private LayoutDocument document;


  }
}

