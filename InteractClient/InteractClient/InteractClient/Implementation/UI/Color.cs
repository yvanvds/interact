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

    public override int R { get => (int)(UIObject.R * 255); }
    public override int G { get => (int)(UIObject.G * 255); }
    public override int B { get => (int)(UIObject.B * 255); }
    public override int A { get => (int)(UIObject.A * 255); }

    public override void SetColor(int r, int g, int b, int a)
    {
      UIObject = new Xamarin.Forms.Color(r / 255.0, g / 255.0, b / 255.0, a / 255.0);
    }

    public Color()
    {
      SetColor(255, 255, 255, 255);
    }

    public Color(int r, int g, int b)
    {
      SetColor(r, g, b, 255);
    }

    public Color(int r, int g, int b, int a)
    {
      SetColor(r, g, b, a);
    }

    public Color(Xamarin.Forms.Color color)
    {
      SetColor((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255), (int)(color.A * 255));
    }
  }
}
