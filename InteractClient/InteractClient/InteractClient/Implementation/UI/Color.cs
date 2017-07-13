using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Implementation.UI
{
  public class Color : Interact.UI.Color
  {
    private Xamarin.Forms.Color UIObject;

    public override object InternalObject => UIObject;

    public override void SetColor(int r, int g, int b, int a)
    {
      UIObject = new Xamarin.Forms.Color(r / 255.0, g / 255.0, b / 255.0, a / 255.0);
    }
  }
}
