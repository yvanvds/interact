using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using System.Windows.Media;
using System.Windows;

namespace InteractServer.Implementation.UI
{
  public class Slider : Interact.UI.Slider
  {
    private System.Windows.Controls.Slider UIObject = new System.Windows.Controls.Slider();
    private Color backgroundColor;

    private Network.OscSender OscSender = null;
    private string OscAddress;

    class Handler
    {
      public string name;
      public object[] arguments;
    }
    private Handler OnChangeHandler;

    public Slider()
    {
      UIObject.Uid = Guid.NewGuid().ToString();
      //UIObject.HorizontalAlignment = HorizontalAlignment.Center;
      //UIObject.VerticalAlignment = VerticalAlignment.Center;
      UIObject.Style = Application.Current.FindResource("FlatSlider") as Style;
      UIObject.Margin = new System.Windows.Thickness(5, 5, 5, 5);

    }

    public override double Minimum { get => UIObject.Minimum; set => UIObject.Minimum = value; }
    public override double Maximum { get => UIObject.Maximum; set => UIObject.Maximum = value; }
    public override double Value { get => UIObject.Value; set => UIObject.Value = value; }

    public override Interact.UI.Color BackgroundColor
    {
      set
      {
        UIObject.Background = new SolidColorBrush((System.Windows.Media.Color)value.InternalObject);
        backgroundColor = new Color((System.Windows.Media.Color)value.InternalObject);
      }
      get => backgroundColor;
    }

    public override object InternalObject => UIObject;

    public override void OnChange(string functionName, params object[] arguments)
    {
      OnChangeHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      UIObject.ValueChanged += OnChangeEvent;
    }

    private void OnChangeEvent(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      OscSender?.Send(OscAddress, (float)UIObject.Value);
      JintEngine.Runner.Engine.InvokeMethod(OnChangeHandler.name, OnChangeHandler.arguments);
    }

    public override void SendOSC(string destination, int port, string address)
    {
      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }
  }
}
