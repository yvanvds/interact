using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CCircle : CDrawable
  {
    public abstract float Size { set; get; }
    public abstract Utility.Coordinate Pos { set; get; }
    public abstract Color Color { set; get; }
    public abstract bool Fill { set; get; }
  }
}
