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
    private Xamarin.Forms.Button UIObject = new Xamarin.Forms.Button();

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
      UIObject.Clicked += JintEngine.Engine.EventHandler.OnClick;
    }

    public override void OnRelease(string functionName, params object[] arguments)
    {
      JintEngine.Engine.EventHandler.RegisterRelease(UIObject.Id, functionName, arguments);
      UIObject.Unfocused += JintEngine.Engine.EventHandler.OnRelease;
    }
  }
}
