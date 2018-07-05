using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.Logic;
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

    private Interact.Logic.Patcher patcher = null;
    private string patcherInletName;

#endregion

    public Button()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigButton.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigButton.Foreground.Get();
      UIObject.BorderColor = Data.Project.Current.ConfigButton.Foreground.Get();
      UIObject.CornerRadius = 0;
      UIObject.BorderWidth = 3;
      UIObject.Margin = new Xamarin.Forms.Thickness(1);

      UIObject.Pressed += OnClickEvent;
      UIObject.Released += OnReleaseEvent;
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
        UIObject.BorderColor = UIObject.TextColor;
        textColor = new Color(UIObject.TextColor);
      }
    }
    #endregion

    #region Interaction
    public override float Pressure => pressure;

    public override Interact.UI.Image Image
    {
      set
      {
        // not implemented on xamarin. Draw the image on top of the button instead. (By adding it to the same grid position)
      }
    }

    public override void OnClick(string functionName, params object[] arguments)
    {
      OnClickHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      
    }

    public override void OnRelease(string functionName, params object[] arguments)
    {
      OnReleaseHandler = new Handler()
      {
        name = functionName,
        arguments = arguments
      };
      
    }

    private void OnClickEvent(object sender, EventArgs e)
    {
      pressure = (e as Interface.PressedEventArgs).Pressure;
      OscSender?.Send(OscAddress, pressure);
      patcher?.PassFloat(pressure, patcherInletName);

      if(OnClickHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnClickHandler.name, OnClickHandler.arguments);
      }
      
    }

    private void OnReleaseEvent(object sender, EventArgs e)
    {
      pressure = 0;
      OscSender?.Send(OscAddress, pressure);
      patcher?.PassFloat(pressure, patcherInletName);

      if(OnReleaseHandler != null)
      {
        JintEngine.Engine.Instance.Invoke(OnReleaseHandler.name, OnReleaseHandler.arguments);
      }
    }

    public override void SendOSC(string destination, int port, string address)
    {
      OscSender = new Network.OscSender();
      OscSender.Init(destination, port);
      OscAddress = address;
    }

    public override void SendToPatcher(Patcher patcher, string inlet)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
