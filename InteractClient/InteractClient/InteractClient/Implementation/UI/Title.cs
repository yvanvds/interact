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
  }
}
