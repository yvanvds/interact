using Interact.Utility;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Implementation.UI.Canvas
{
  public class CLine : Interact.UI.Canvas.CLine
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

    Coordinate start;
    public override Coordinate Start { get => start; set => start = value; }

    Coordinate end;
    public override Coordinate End { get => end; set => end = value; }

    public override float Width { get => paint.StrokeWidth; set => paint.StrokeWidth = value; }

    public CLine()
    {
      paint = new SKPaint();
      Color = new Color(255, 255, 255);
      paint.Style = SKPaintStyle.Stroke;
    }

    public CLine(Coordinate start, Coordinate end) : this()
    {
      this.start = start;
      this.end = end;
    }

    public override void Draw(SKCanvas canvas)
    {
      canvas.DrawLine(start.X, -start.Y, end.X, -end.Y, paint);
    }
  }
}
