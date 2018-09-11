using ActiproSoftware.Windows.Input;
using InteractServer.Utils;
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
	public delegate bool Validator(string value);

	public partial class GetString : MetroWindow
	{
		public GetString(string title)
		{
			InitializeComponent();
			this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			Title = title;
			this.DataContext = this;
			this.ShowCloseButton = false;
			this.ShowIconOnTitleBar = false;
			this.ShowMaxRestoreButton = false;
			this.ShowMinButton = false;

			StringResult.Text = string.Empty;
			StringResult.Focus();
		}

		private string result = string.Empty;
		public string Result => result;

		public Validator Validator = null;

		private ICommand enterPressed;
		public ICommand EnterPressed
		{
			get
			{
				return enterPressed 
					?? (enterPressed = new ActionCommand(()=>
					{
						DialogResult = true;
						result = StringResult.Text;
						Close();
					}));
			}
		}

		private ICommand escapePressed;
		public ICommand EscapePressed
		{
			get
			{
				return escapePressed
					?? (escapePressed = new ActionCommand(() =>
					{
						DialogResult = false;
						result = string.Empty;
						Close();
					}));
			}
		}
	}
}
