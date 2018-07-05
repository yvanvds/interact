using System;
using System.Collections.Generic;
using System.Text;
using Interact.UI.Canvas;
using SkiaSharp;

namespace InteractClient.Implementation.UI.Canvas
{
  public class CLayer : Interact.UI.Canvas.CLayer
  {
    Dictionary<int, CDrawable> objects = new Dictionary<int, CDrawable>();

    public override void Add(int ID, CDrawable obj)
    {
      objects.Add(ID, obj);
    }

    public override void Draw(SKCanvas canvas)
    {
      foreach (var value in objects.Values)
      {
        value.Draw(canvas);
      }
    }

    public override CDrawable Get(int ID)
    {
      return objects[ID];
    }

    public override void Remove(int ID)
    {
      objects.Remove(ID);
    }
  }
}
