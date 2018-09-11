using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InteractClient.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Performer : ContentPage
	{
		public Performer (OSCGui_Forms.OscGuiView view)
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
			Content = view;
		}

		public void SetView(OSCGui_Forms.OscGuiView view)
		{
			Content = view;
		}

		public void Stop()
		{
			Content = null;
			Navigation.PopAsync();
		}

		protected override bool OnBackButtonPressed()
		{
			if (Global.CurrentProject != null)
			{
				Global.CurrentProject.Stop();
			}
			return base.OnBackButtonPressed();
		}
	}
}