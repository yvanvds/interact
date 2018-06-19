using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CDrawable
  {
    public abstract void Draw(SkiaSharp.SKCanvas canvas);
  }
}
