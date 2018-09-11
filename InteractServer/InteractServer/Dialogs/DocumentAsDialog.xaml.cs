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
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.Dialogs
{
	/// <summary>
	/// Interaction logic for DocumentAsDialog.xaml
	/// </summary>
	public partial class DocumentAsDialog : MetroWindow
	{
		LayoutDocument document;

		public DocumentAsDialog(string title, object content)
		{
			InitializeComponent();
			Title = title;
			Frame frame = new Frame();
			frame.Content = content;
			this.document = new LayoutDocument();
			this.document.Content = frame;
			Dock.InsertChildAt(0, this.document);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Dock.RemoveChild(document);
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
