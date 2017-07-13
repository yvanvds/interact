using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shared;
using InteractServer.Models;
using InteractServer.Views;
using InteractServer.Controls;

namespace InteractServer.Pages
{
  /// <summary>
  /// Interaction logic for ScriptModelPage.xaml
  /// </summary>
  public partial class ScriptPage : BasePage
  {

    public ScriptPage(ScreenView view) : base(view)
    {
      InitializeComponent();
      Editor.Text = (view.Screen.ContentObj as ScreenContent.Script).Content;
      Editor.ContentChanged += Editor_ContentChanged;
      Editor.ServerSide = false;

      Editor.InitIntellisense();
      Editor.ScriptName = view.Screen.Name;
    }

    public ScriptPage(ServerScriptView view) : base(view)
    {
      InitializeComponent();
      Editor.Text = view.ServerScript.Content;
      Editor.ContentChanged += Editor_ContentChanged;
      Editor.ServerSide = true;

      Editor.InitIntellisense();
      Editor.ScriptName = view.ServerScript.Name;
    }

    public override void Save()
    {
      if(ServerSide)
      {
        ServerScriptView.ServerScript.Content = Editor.Text;
      } else
      {
        (ScreenView.Screen.ContentObj as ScreenContent.Script).Content = Editor.Text;
      }
    }

    public override void DiscardChanges()
    {
      if(ServerSide)
      {
        Editor.Text = ServerScriptView.ServerScript.Content;
      } else
      {
        Editor.Text = (ScreenView.Screen.ContentObj as ScreenContent.Script).Content;
      }
    }

    private void Editor_ContentChanged(object sender, EventArgs e)
    {
      if(ServerSide)
      {
        ServerScriptView.Taint();
      } else
      {
        ScreenView.Taint();
      }
    }

    public CodeEditor GetCodeEditor()
    {
      return Editor;
    }
  }
}
