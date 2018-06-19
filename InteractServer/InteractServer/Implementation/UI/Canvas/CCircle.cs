using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using Interact.Utility;
using SkiaSharp;

namespace InteractServer.Implementation.UI.Canvas
{
  public class CCircle : Interact.UI.Canvas.CCircle
  {
    float size = 0f;
    public override float Size { get => size; set => size = value; }

    Coordinate pos = new Coordinate();
    public override Coordinate Pos { get => pos; set => pos = value; }

    SKPaint paint;

    Interact.UI.Color color;
    public override Interact.UI.Color Color
    {
      get => color;

      set
      {
        color = value;
        paint.Color = new SKColor((byte)color.R, (byte)color.G, (byte)color.B, (byte)color.A);
      }
    }

    bool fill;
    public override bool Fill
    {
      get => fill;
      set
      {
        fill = value;
        if(fill)
        {
          paint.Style = SKPaintStyle.Fill;
          paint.StrokeWidth = 0;
        } else
        {
          paint.Style = SKPaintStyle.Stroke;
          paint.StrokeWidth = 2f;
        }
      }
    }

    public CCircle()
    {
      paint = new SKPaint();
      Color = new Color(255, 255, 255);
      Fill = true;
    }

    public CCircle(Coordinate pos, float size) : this()
    {
      Pos = pos;
      Size = size;
    }

    public override void Draw(SKCanvas canvas)
    {
      canvas.DrawCircle(Pos.X, -Pos.Y, size, paint);
    }
  }
}
