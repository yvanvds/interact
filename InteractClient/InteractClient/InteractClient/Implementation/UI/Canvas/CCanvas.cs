using Interact.UI;
using Interact.UI.Canvas;
using Interact.Utility;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Implementation.UI.Canvas
{
  public class CCanvas : Interact.UI.Canvas.CCanvas
  {
    private class Canvas : SkiaSharp.Views.Forms.SKCanvasView
    {
      public SKColor backgroundColor = new SKColor();
      public string onTouchDown = "", onTouchUp = "", onTouchMove = "";

      public List<CDrawable> Objects = new List<CDrawable>();

      public new float Width;
      public new float Height;

      private Coordinate center;

      public Canvas()
      {
        this.Touch += TouchCallback;
        this.EnableTouchEvents = true;
      }

      protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
      {
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;
        Width = e.Info.Size.Width;
        Height = e.Info.Size.Height;

        center = new Coordinate(e.Info.Width / 2f, e.Info.Height / 2f);

        canvas.Clear(backgroundColor);
        canvas.Translate(Width / 2f, Height / 2f);
        canvas.Scale(Height / 1000f);

        foreach (var obj in Objects)
        {
          obj.Draw(canvas);
        }
      }

      protected void TouchCallback(object sender, SKTouchEventArgs e)
      {
        switch(e.ActionType)
        {
          case SKTouchAction.Pressed:
            if(onTouchDown.Length > 0)
            {
              JintEngine.Engine.Instance.Invoke(
                onTouchDown, 
                e.Id, 
                new Coordinate(
                  (e.Location.X - center.X) / (Height / 1000f), 
                  -(e.Location.Y - center.Y) / (Height / 1000f)
                )
              );
              e.Handled = true;
            }
            break;
          case SKTouchAction.Cancelled:
          case SKTouchAction.Exited:
          case SKTouchAction.Released:
            if(onTouchUp.Length > 0)
            {
              JintEngine.Engine.Instance.Invoke(
                onTouchUp,
                e.Id,
                new Coordinate(
                  (e.Location.X - center.X) / (Height / 1000f),
                  -(e.Location.Y - center.Y) / (Height / 1000f)
                )
              );
              e.Handled = true;
            }
            break;
          case SKTouchAction.Moved:
            if(onTouchMove.Length > 0)
            {
              JintEngine.Engine.Instance.Invoke(
                onTouchMove, 
                e.Id, 
                new Coordinate(
                  (e.Location.X - center.X) / (Height / 1000f),
                  -(e.Location.Y - center.Y) / (Height / 1000f)
                )
              );
              e.Handled = true;
            }
            break;
        }
        e.Handled = true;
      }
    }
    private Canvas canvas = new Canvas();

    public override Coordinate Mouse => new Coordinate();

    public override float Width => (float)canvas.Width;

    public override float Height => canvas.Height;

    public override object InternalObject => canvas;

    public override Interact.UI.Color BackgroundColor
    {
      get => new Color(canvas.backgroundColor.Red, canvas.backgroundColor.Green, canvas.backgroundColor.Blue);
      set => canvas.backgroundColor = new SKColor((byte)value.R, (byte)value.G, (byte)value.B);
    }

    public override void Init()
    {
      Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
      {
        canvas.InvalidateSurface();
        return true;
      });
    }

    public override void OnTouchDown(string functionName)
    {
      canvas.onTouchDown = functionName;
    }

    public override void OnTouchUp(string functionName)
    {
      canvas.onTouchUp = functionName;
    }

    public override void OnTouchMove(string functionName)
    {
      canvas.onTouchMove = functionName;
    }

    public override void Add(CDrawable obj)
    {
      canvas.Objects.Add(obj);
    }

    public override void Remove(CDrawable obj)
    {
      canvas.Objects.Remove(obj);
    }
  }
}
