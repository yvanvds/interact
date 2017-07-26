using InteractClient.Interface;
using InteractClient.UWP.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CCButton), typeof(CCButtonRenderer))]
namespace InteractClient.UWP.Implementation
{
  public class CCButtonRenderer : ButtonRenderer
  {
    CCButton ccButton = null;

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
    {
      base.OnElementChanged(e);

      ccButton = e.NewElement as CCButton;

      if(Control != null)
      {
        Control.ClickMode = Windows.UI.Xaml.Controls.ClickMode.Press;
        Control.Click += Control_Click;
        Control.PointerPressed += Control_PointerPressed;
        Control.PointerReleased += Control_PointerReleased;
        Control.PointerMoved += Control_PointerMoved;
      }
    }

    private void Control_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
      if(ccButton != null)
      {
        ccButton.OnPressed(1);
      }
    }

    private void Control_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void Control_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
      if(ccButton != null)
      {
        ccButton.OnReleased();
      }
    }

    private void Control_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
      if(ccButton != null)
      {
        Windows.UI.Input.PointerPoint point = e.GetCurrentPoint(Control);
        float pressure = point.Properties.Pressure;
        ccButton.OnPressed(pressure);
        CapturePointer(e.Pointer);
      }
    }
  }
}
