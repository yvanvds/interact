﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace InteractClient.Droid
{
  [Activity(Label = "Interact", Icon = "@drawable/connect", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);
      Xamarin.Forms.DependencyService.Register<Implementation.OscImplementation>();
      Xamarin.Forms.DependencyService.Register<Implementation.OscReceiverImplementation>();
      Xamarin.Forms.DependencyService.Register<Implementation.SensorImplementation>();
      LoadApplication(new App());
    }
  }
}

