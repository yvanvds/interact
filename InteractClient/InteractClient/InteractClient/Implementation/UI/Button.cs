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
    private Interface.CCButton UIObject = new Interface.CCButton();
    private Color backgroundColor;
    private Color textColor;

    public Button()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigButton.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigButton.Foreground.Get();
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override object InternalObject => UIObject;

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

    public override void OnClick(string functionName, params object[] arguments)
    {
      JintEngine.Engine.EventHandler.RegisterClick(UIObject.Id, functionName, arguments);
      UIObject.Pressed += JintEngine.Engine.EventHandler.OnClick;
    }

    public override void OnRelease(string functionName, params object[] arguments)
    {
      JintEngine.Engine.EventHandler.RegisterRelease(UIObject.Id, functionName, arguments);
      UIObject.Released += JintEngine.Engine.EventHandler.OnRelease;
    }
  }
}
