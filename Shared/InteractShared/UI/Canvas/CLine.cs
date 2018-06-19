using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CLine : CDrawable
  {
    public abstract Utility.Coordinate Start { get; set; }
    public abstract Utility.Coordinate End { get; set; }
    public abstract Color Color { get; set; }
    public abstract float Width { get; set; }
  }
}
