using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.UI.Canvas
{
  public abstract class CLayer : CDrawable
  {
    public abstract void Add(int ID, CDrawable obj);
    public abstract CDrawable Get(int ID);
    public abstract void Remove(int ID);
  }
}
