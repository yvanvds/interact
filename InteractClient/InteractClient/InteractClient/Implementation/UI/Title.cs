using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.UI
{
  public class Title : Interact.UI.Title
  {
    private Xamarin.Forms.Label UIObject = new Xamarin.Forms.Label();
    private Color backgroundColor;
    private Color textColor;

    public Title()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigTitle.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigTitle.Foreground.Get();
      UIObject.FontSize = Data.Project.Current.ConfigTitle.FontSize;

      UIObject.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
      UIObject.VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center;
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override object InternalObject => UIObject;

    public override Interact.UI.Color TextColor
    {
      get => textColor;
      set
      {
        UIObject.TextColor = (Xamarin.Forms.Color)(value.InternalObject);
        textColor = new Color(UIObject.TextColor);
      }
    }

    public override Interact.UI.Color BackgroundColor
    {
      get => backgroundColor;
      set
      {
        UIObject.BackgroundColor = (Xamarin.Forms.Color)(value.InternalObject);
        backgroundColor = new Color(UIObject.BackgroundColor);
      }
    }
  }
}
