using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CRect : CDrawable
  {
    public abstract Utility.Coordinate Pos { get; set; }
    public abstract float Width { get; set; }
    public abstract float Height { get; set; }
    public abstract Color Color { get; set; }
    public abstract bool Fill { set; get; }
  }
}
