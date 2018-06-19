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
  public class CRect : Interact.UI.Canvas.CRect
  {

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
        if (fill)
        {
          paint.Style = SKPaintStyle.Fill;
          paint.StrokeWidth = 0;
        }
        else
        {
          paint.Style = SKPaintStyle.Stroke;
          paint.StrokeWidth = 2f;
        }
      }
    }

    Coordinate pos;
    public override Coordinate Pos { get => pos; set => pos = value; }

    float width;
    public override float Width { get => width; set => width = value; }

    float height;
    public override float Height { get => height; set => height = value; }

    public CRect()
    {
      paint = new SKPaint();
      Color = new Color(255, 255, 255);
      Fill = true;
    }

    public CRect(Coordinate position, float width, float height) : this()
    {
      Pos = position;
      Width = width;
      Height = height;
    }

    public override void Draw(SKCanvas canvas)
    {
      canvas.DrawRect(Pos.X, -Pos.Y - Height, Width, Height, paint);
    }
  }
}
