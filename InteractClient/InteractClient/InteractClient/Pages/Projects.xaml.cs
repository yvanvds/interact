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
	public partial class Projects : ContentPage
	{
		Project.Manager Manager = new Project.Manager();

		public Projects ()
		{
			InitializeComponent ();

			if(Device.RuntimePlatform == Device.Android)
			{
				BackButton.IsVisible = false;
				BackButton.HeightRequest = 0;
			} else
			{
				BackButton.IsVisible = true;
				BackButton.HeightRequest = 80;
			}
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await Manager.Load();
			ProjectList.ItemsSource = Manager.List;

			
		}

		private void ButtonRun_Clicked(object sender, EventArgs e)
		{
			/*var button = sender as Button;
			var projectInfo = button.BindingContext as ProjectInfo;
			await Project.SetCurrent(new Guid(projectInfo.Guid), -1);
			var screen = Project.Current.GetScreen(projectInfo.FirstScreen);
			JintEngine.Engine.Instance.SetActivePage(this);
			JintEngine.Engine.Instance.StartScreen(screen.ID);*/
		}

		private async void ButtonDelete_ClickedAsync(object sender, EventArgs e)
		{
			var answer = await DisplayAlert("Delete", "Really delete this project?", "Yes", "No");
			if (answer)
			{
				var button = sender as Button;
				var projectInfo = button.BindingContext as Project.Info;
				await Manager.DeleteFromDisk(projectInfo);
			}
		}

		public void PushModelPage()
		{
			//Navigation.PushAsync(new ModelPage());
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}
	}
}