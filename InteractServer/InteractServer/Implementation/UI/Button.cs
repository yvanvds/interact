using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Interact.UI;
using System.ComponentModel;
using System.Windows;

namespace InteractServer.Implementation.UI
{
  public class Button : Interact.UI.Button
  {
    private System.Windows.Controls.Button UIObject = new System.Windows.Controls.Button();
    private Color backgroundColor;
    private Color textColor;
    private float pressure;

    private DependencyPropertyDescriptor descriptor;
    private Network.OscSender OscSender = null;
    private string OscAddress;

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
      UIObject.Style = Application.Current.FindResource("SquareButtonStyle") as Style;
      UIObject.Uid = Guid.NewGuid().ToString();

      descriptor = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Button.IsPressedProperty, typeof(System.Windows.Controls.Button));
      descriptor.AddValueChanged(UIObject, new EventHandler(IsPressedChanged));
    }

    private void IsPressedChanged(object sender, EventArgs e) 
    {
      if(UIObject.IsPressed)
      {
        pressure = 1;
        OscSender?.Send(OscAddress, pressure);

        if (OnClickHandler != null)
        {
          JintEngine.Runner.Engine.InvokeMethod(OnClickHandler.name, OnClickHandler.arguments);
        }
      } else
      {
        pressure = 0;
        OscSender?.Send(OscAddress, pressure);

        if (OnReleaseHandler != null)
        {
          JintEngine.Runner.Engine.InvokeMethod(OnReleaseHandler.name, OnReleaseHandler.arguments);
        }
      }
      OscSender?.Send(OscAddress, pressure);
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

    // Pressurre cannot be detected in WPF
    public override float Pressure => pressure;

    public override void OnClick(string functionName, params object[] args)
    {
      OnClickHandler = new Handler
      {
        name = functionName,
        arguments = args
      };
    }

    public override void OnRelease(string functionName, params object[] args)
    {
      OnReleaseHandler = new Handler
      {
        name = functionName,
        arguments = args
      };
    }

    public override void SendOSC(string destination, int port, string address)
    {
      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }
  }
}
