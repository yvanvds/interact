using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CCanvas : View
  {
    public abstract void Init();

    public abstract void OnTouchDown(string functionName);
    public abstract void OnTouchUp(string functionName);
    public abstract void OnTouchMove(string functionName);

    public abstract Utility.Coordinate Mouse { get; }
    public abstract float Width { get; }
    public abstract float Height { get; }

    public abstract void Add(CDrawable obj);
    public abstract void Remove(CDrawable obj);
  }
}
