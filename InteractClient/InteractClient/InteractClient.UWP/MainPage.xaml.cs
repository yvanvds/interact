using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace InteractClient.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();

			//Global.Yse = new YSE.YseInterface(OnLogMessage);
			//Global.Yse.Log.Level = IYse.ERROR_LEVEL.DEBUG;

			InteractClient.IO.FileSystem.Init(new InteractClient.UWP.FileSystem.FileSystem());
			Global.Compiler = new Compiler.Compiler();
			LoadApplication(new InteractClient.App());
		}

		private void OnLogMessage(string message)
		{
			System.Diagnostics.Debug.WriteLine("Yse Sound Engine: " + message);
		}
	}
}
