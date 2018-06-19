using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Interact.UI;
using Interact.UI.Canvas;
using InteractServer.JintEngine;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace InteractServer.Implementation.UI.Canvas
{
  class CCanvas : Interact.UI.Canvas.CCanvas
  {
    private class Canvas : SkiaSharp.Views.WPF.SKElement
    {
      public SKColor backgroundColor = new SKColor();
      public string onTouchDown = "", onTouchUp = "", onTouchMove = "";
      public Interact.Utility.Coordinate Mouse;

      public List<CDrawable> Objects = new List<CDrawable>();

      public new float Width;
      public new float Height;

      protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
      {
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;
        Width = e.Info.Size.Width;
        Height = e.Info.Size.Height;

        Interact.Utility.Coordinate center = new Interact.Utility.Coordinate(e.Info.Width / 2f, e.Info.Height / 2f);

        canvas.Clear(backgroundColor);
        canvas.Translate(Width / 2f, Height / 2f);
        canvas.Scale(Height / 1000f);

        foreach (var obj in Objects)
        {
          obj.Draw(canvas);
        }
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
        Mouse.X = (float)(e.GetPosition(this).X - (Width / 2f)) / (Height / 1000f);
        Mouse.Y = (float)(-e.GetPosition(this).Y + (Height / 2f)) / (Height / 1000f);

        if (Runner.Engine != null && onTouchMove.Length > 0)
        {
          Runner.Engine.InvokeMethod(onTouchMove);
        }
      }

      protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
      {
        if (Runner.Engine != null && onTouchUp.Length > 0)
        {
          Runner.Engine.InvokeMethod(onTouchUp, 0);
        }
      }

      protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
      {
        if (Runner.Engine != null && onTouchUp.Length > 0)
        {
          Runner.Engine.InvokeMethod(onTouchUp, 1);
        }
      }

      protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
      {
        if (Runner.Engine != null && onTouchDown.Length > 0)
        {
          Runner.Engine.InvokeMethod(onTouchDown, 0);
        }
      }

      protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
      {
        if (Runner.Engine != null && onTouchDown.Length > 0)
        {
          Runner.Engine.InvokeMethod(onTouchDown, 1);
        }
      }
    }
    private Canvas canvas = new Canvas();

    private DispatcherTimer updateCanvas = new DispatcherTimer(DispatcherPriority.Render);


    public override Interact.UI.Color BackgroundColor {
      set {

        canvas.backgroundColor = new SKColor((byte)value.R, (byte)value.G, (byte)value.B, (byte)value.A);
      }

      get
      {
        return new Color(canvas.backgroundColor.Red, canvas.backgroundColor.Green, canvas.backgroundColor.Blue);
      }
    }

    public override Interact.Utility.Coordinate Mouse
    {
      get => canvas.Mouse;
    }

    public override object InternalObject => canvas;

    public override float Width => (float)canvas.Width;
    public override float Height => (float)canvas.Height;

    public override void Init()
    {
      updateCanvas.Interval = new TimeSpan(0, 0, 0, 0, 50);
      updateCanvas.Tick += new EventHandler(UpdateCanvasElapsed);
      updateCanvas.Start();
    }

    private void UpdateCanvasElapsed(object sender, EventArgs e)
    {
      canvas.InvalidateVisual();
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
