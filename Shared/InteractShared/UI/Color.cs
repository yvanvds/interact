using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI
{
  public abstract class Color
  {
    public abstract object InternalObject { get; }

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

    public abstract void SetColor(int r, int g, int b, int a);

  }
}
