using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;

namespace InteractClient.Implementation.UI
{
  public class Button : Interact.UI.Button
  {
    
    #region Declarations

    private Interface.CCButton UIObject = new Interface.CCButton();
    private Color backgroundColor;
    private Color textColor;
    private float pressure;

    private Network.OscSender OscSender = null;
    private string OscAddress;

    // Stores handlers for javascript callbacks
    class Handler
    {
      public string name;
      public object[] arguments;
    }

    Handler OnClickHandler;
    Handler OnReleaseHandler;

#endregion

    public Button()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigButton.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigButton.Foreground.Get();
    }

    public override object InternalObject => UIObject;

    #region Appearance
    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override Interact.UI.Color BackgroundColor {
      get => backgroundColor;
      set
      {
        UIObject.BackgroundColor = (Xamarin.Forms.Color)(value.InternalObject);
        backgroundColor = new Color(UIObject.BackgroundColor);
      }
    }

    public override Interact.UI.Color TextColor {
      get => textColor;
      set
      {
        UIObject.TextColor = (Xamarin.Forms.Color)(value.InternalObject);
        textColor = new Color(UIObject.TextColor);
      }
    }
    #endregion

    #region Interaction
    public override float Pressure => pressure;

    public override void OnClick(string functionName, params object[] arguments)
    {
      OnClickHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      UIObject.Pressed += OnClickEvent;
    }

    public override void OnRelease(string functionName, params object[] arguments)
    {
      OnReleaseHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      UIObject.Released += OnReleaseEvent;
    }

    private void OnClickEvent(object sender, EventArgs e)
    {
      pressure = (e as Interface.PressedEventArgs).Pressure;
      OscSender?.Send(OscAddress, pressure);
      JintEngine.Engine.Instance.Invoke(OnClickHandler.name, OnClickHandler.arguments);
    }

    private void OnReleaseEvent(object sender, EventArgs e)
    {
      pressure = 0;
      OscSender?.Send(OscAddress, pressure);
      JintEngine.Engine.Instance.Invoke(OnReleaseHandler.name, OnReleaseHandler.arguments);
    }

    public override void SendOSC(string destination, int port, string address)
    {
      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }

#endregion
  }
}
