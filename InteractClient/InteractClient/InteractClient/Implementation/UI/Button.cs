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

    public Button()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigButton.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigButton.Foreground.Get();
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override object InternalObject => UIObject;

    public override void SetColor(Interact.UI.Color color)
    {
      UIObject.BackgroundColor = (Xamarin.Forms.Color)(color.InternalObject);
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
