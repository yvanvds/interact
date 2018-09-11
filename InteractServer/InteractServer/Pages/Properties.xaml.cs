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
using OscTree;

namespace InteractServer.Pages
{
	/// <summary>
	/// Interaction logic for Properties.xaml
	/// </summary>
	public partial class Properties : Page, OscGuiControl.IOscRoutePicker
	{
		public static Properties Handle = null;
		public static OscTree.Object Object = null;

		public Properties()
		{
			InitializeComponent();
			GuiInspector.OscRoot = Osc.Tree.Root;
			GuiInspector.CustomRoutePicker = this;
			Handle = this;
		}

		public void SetSelected(object obj)
		{
			GuiInspector.Inspect(obj);
		}

		public OscGuiControl.OscInspectorGui GetInspector()
		{
			return GuiInspector;
		}

		public void SetRoutes(Routes routes)
		{
			routes.UpdateScreenNames(Osc.Tree.Root);
			var dialog = new Dialogs.RouteInspector(Osc.Tree.Root);
			dialog.List = routes;
			if(GuiInspector.CurrentObject is IOscObject)
			{
				dialog.Origin = (GuiInspector.CurrentObject as IOscObject).OscObject;
			}
			
			dialog.ShowDialog();

		}
	}
}
