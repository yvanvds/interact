using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using Xamarin.Forms;

namespace InteractClient.Implementation.UI
{
  class Slider : Interact.UI.Slider
  {
    private Xamarin.Forms.Slider UIObject = new Xamarin.Forms.Slider();
    private Color backgroundColor;

    private Network.OscSender OscSender = null;
    private string OscAddress;

    class Handler
    {
      public string name;
      public object[] arguments;
    }

    Handler OnChangeHandler;

    public override double Minimum { get => UIObject.Minimum; set => UIObject.Minimum = value; }
    public override double Maximum { get => UIObject.Maximum; set => UIObject.Maximum = value; }
    public override double Value { get => UIObject.Value; set => UIObject.Value = value; }

    public override Interact.UI.Color BackgroundColor
    {
      get => backgroundColor;
      set
      {
        UIObject.BackgroundColor = (Xamarin.Forms.Color)(value.InternalObject);
        backgroundColor = new Color(UIObject.BackgroundColor);
      }
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

    private void OnChangeEvent(object sender, ValueChangedEventArgs e)
    {
      Value = e.NewValue;
      OscSender?.Send(OscAddress, (float)Value);
      JintEngine.Engine.Instance.Invoke(OnChangeHandler.name, OnChangeHandler.arguments);
    }

    public override void SendOSC(string destination, int port, string address)
    {
      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }
  }
}
