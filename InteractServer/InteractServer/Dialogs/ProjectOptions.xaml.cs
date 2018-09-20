using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InteractServer.Dialogs
{
	/// <summary>
	/// Interaction logic for ProjectOptions.xaml
	/// </summary>
	public partial class ProjectOptions : MetroWindow
	{
		string projectName;
		bool projectNameChanged;
		public string ProjectName
		{
			get => projectName;
			set
			{
				projectName = value;
				projectNameChanged = true;
			}
		}

		string projectFirstGui;
		bool projectFirstGuiChanged;
		public string ProjectFirstGui
		{
			get => projectFirstGui;
			set
			{
				projectFirstGui = value;
				projectFirstGuiChanged = true;
			}
		}

		string resolumeIP;
		bool resolumeIPChanged;
		public string ResolumeIP
		{
			get => resolumeIP;
			set
			{
				resolumeIP = value;
				resolumeIPChanged = true;
			}
		}

		string resolumePort;
		bool resolumePortChanged;
		public string ResolumePort
		{
			get => resolumePort;
			set
			{
				resolumePort = value;
				resolumePortChanged = true;
			}
		}

		public ProjectOptions()
		{
			InitializeComponent();

			ProjectName = Project.Project.Current?.Name;
			ProjectFirstGui = Project.Project.Current?.FirstClientGui;

			ResolumeIP = Project.Project.Current?.ResolumeIP;
			ResolumePort = Project.Project.Current?.ResolumePort.ToString();

			projectNameChanged = false;
			projectFirstGuiChanged = false;

			resolumeIPChanged = false;
			resolumePortChanged = false;

			this.DataContext = this;
			TabGeneral.DataContext = this;
			TabResolume.DataContext = this;
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			if(projectNameChanged)
			{
				Project.Project.Current.Name = ProjectName;
			}
			
			if(projectFirstGuiChanged)
			{
				Project.Project.Current.FirstClientGui = ProjectFirstGui;
			}
			
			if(resolumePortChanged || resolumeIPChanged)
			{
				Project.Project.Current.ResolumeIP = ResolumeIP;
				Project.Project.Current.ResolumePort = Convert.ToInt32(ResolumePort);
				Network.Resolume.Handle?.DisConnect();
				Network.Resolume.Handle?.Connect(ResolumeIP, Convert.ToInt32(ResolumePort));
			}

			Close();
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
