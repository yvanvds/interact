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

    public override void SetColor(int r, int g, int b, int a)
    {
      UIObject = new System.Windows.Media.Color();
      UIObject.R = (byte)r;
      UIObject.G = (byte)g;
      UIObject.B = (byte)b;
      UIObject.A = (byte)a;
    }
  }
}
