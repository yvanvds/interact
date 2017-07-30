using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.UI
{
  public class Image : Interact.UI.Image
  {
    private Xamarin.Forms.Image UIObject = new Xamarin.Forms.Image();
    private Color backgroundColor;

    public Image()
    {
      UIObject.HorizontalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand;
      UIObject.VerticalOptions = Xamarin.Forms.LayoutOptions.CenterAndExpand;
    }

    public override object InternalObject => UIObject;

    public override void Set(string ImageName)
    {
      UIObject.Source = Data.Project.Current.GetImage(ImageName).ImageSource;
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
