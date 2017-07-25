using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using InteractClient.Droid.Implementation;
using InteractClient.Interface;

[assembly: ExportRenderer(typeof(CCButton), typeof(CCButtonRenderer))]
namespace InteractClient.Droid.Implementation
{
  public class CCButtonRenderer : ButtonRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
    {
      base.OnElementChanged(e);

      var ccButton = e.NewElement as CCButton;

      var thisButton = Control as Android.Widget.Button;

      thisButton.Touch += (object sender, TouchEventArgs args) =>
      {
        
        if(args.Event.Action == MotionEventActions.Down)
        {
          ccButton.OnPressed(args.Event.GetPressure(0));
        }
        else if (args.Event.Action == MotionEventActions.Up)
        {
          ccButton.OnReleased();
        }
        else if (args.Event.Action == MotionEventActions.Move)
        {
          
        }
      };
    }
  }
}