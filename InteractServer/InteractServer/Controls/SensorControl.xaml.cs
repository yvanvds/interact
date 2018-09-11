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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractServer.Controls
{
	/// <summary>
	/// Interaction logic for SensorControl.xaml
	/// </summary>
	public partial class SensorControl : UserControl
	{
		public SensorControl()
		{
			InitializeComponent();
		}

		public string ToJSON()
		{
			return string.Empty;
		}

		public void LoadJSON(string content)
		{

		}

		public bool NeedsSaving()
		{
			return false;
		}
	}
}
