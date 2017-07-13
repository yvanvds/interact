using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.UI
{
  public class Text : Interact.UI.Text
  {
    private Xamarin.Forms.Label UIObject = new Xamarin.Forms.Label();

    public Text()
    {
      UIObject.BackgroundColor = Data.Project.Current.ConfigText.Background.Get();
      UIObject.TextColor = Data.Project.Current.ConfigText.Foreground.Get();
      UIObject.FontSize = Data.Project.Current.ConfigText.FontSize;

      UIObject.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
      UIObject.VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center;
    }

    public override string Content { get => UIObject.Text; set => UIObject.Text = value; }

    public override object InternalObject => UIObject;
  }
}
