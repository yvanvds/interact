using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Interact.UI;
using System.ComponentModel;

namespace InteractServer.Implementation.UI
{
  public class Button : Interact.UI.Button
  {
    private System.Windows.Controls.Button UIObject = new System.Windows.Controls.Button();
    private Color backgroundColor;
    private Color textColor;
    private DependencyPropertyDescriptor descriptor;

    class Handler
    {
      public string name;
      public object[] arguments;
    }

    private Handler OnClickHandler;
    private Handler OnReleaseHandler;

    public Button()
    {
      UIObject.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Background);
      UIObject.Foreground = new SolidColorBrush(Global.ProjectManager.Current.ConfigButton.Foreground);
      UIObject.Uid = Guid.NewGuid().ToString();

      descriptor = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Button.IsPressedProperty, typeof(System.Windows.Controls.Button));
      descriptor.AddValueChanged(UIObject, new EventHandler(IsPressedChanged));
    }

    private void IsPressedChanged(object sender, EventArgs e) 
    {
      if (UIObject.IsPressed && OnClickHandler != null)
      {
        JintEngine.Runner.Engine.InvokeMethod(OnClickHandler.name, OnClickHandler.arguments);
      } else if (!UIObject.IsPressed && OnReleaseHandler != null)
      {
        JintEngine.Runner.Engine.InvokeMethod(OnReleaseHandler.name, OnReleaseHandler.arguments);
      }
    }

    public override string Content { get => UIObject.Content as string; set => UIObject.Content = value; }

    public override object InternalObject => UIObject;

    public override Interact.UI.Color TextColor
    {
      set
      {
        UIObject.Foreground = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        textColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => textColor;
    }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => backgroundColor;
    }

    public override void OnClick(string functionName, params object[] args)
    {
      OnClickHandler = new Handler
      {
        name = functionName,
        arguments = args
      };
      //JintEngine.Runner.EventHandler.RegisterClick(UIObject.Uid, functionName, arguments);
      //UIObject.Click += JintEngine.Runner.EventHandler.OnClick;
    }

    public override void OnRelease(string functionName, params object[] args)
    {
      OnReleaseHandler = new Handler
      {
        name = functionName,
        arguments = args
      };
      //JintEngine.Runner.EventHandler.RegisterRelease(UIObject.Uid, functionName, arguments);
      //UIObject.IsPressed += JintEngine.Runner.EventHandler.OnRelease;
    }
  }
}
