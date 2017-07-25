using InteractClient.Interface;
using InteractClient.iOS.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CCButton), typeof(CCButtonRenderer))]
namespace InteractClient.iOS.Implementation
{
  public class CCButtonRenderer : ButtonRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
    {
      base.OnElementChanged(e);

      var ccButton = e.NewElement as CCButton;
      var thisButton = Control as UIButton;
      thisButton.TouchDown += delegate
      {
        ccButton.OnPressed(0);
      };
      thisButton.TouchUpInside += delegate
      {
        ccButton.OnReleased();
      };
    }
  }

}
