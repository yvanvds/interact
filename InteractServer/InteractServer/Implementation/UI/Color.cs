using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Implementation.UI
{
  class Color : Interact.UI.Color
  {
    private System.Windows.Media.Color UIObject;

    public override object InternalObject => UIObject;

    public override int R { get => UIObject.R; }
    public override int G { get => UIObject.G; }
    public override int B { get => UIObject.B; }
    public override int A { get => UIObject.A; }

    public override void SetColor(int r, int g, int b, int a)
    {
      UIObject = new System.Windows.Media.Color();
      UIObject.R = (byte)r;
      UIObject.G = (byte)g;
      UIObject.B = (byte)b;
      UIObject.A = (byte)a;
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

    public Color(System.Windows.Media.Color color)
    {
      SetColor(color.R, color.G, color.B, color.A);
    }
  }
}
