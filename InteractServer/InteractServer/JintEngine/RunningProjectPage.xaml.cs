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
using Xceed.Wpf.AvalonDock.Layout;

namespace InteractServer.JintEngine
{
  /// <summary>
  /// Interaction logic for RunningProjectPage.xaml
  /// </summary>
  public partial class RunningProjectPage : Page
  {
    LayoutDocument document; 

    public Implementation.UI.Grid PageRoot = null;

    public RunningProjectPage()
    {
      InitializeComponent();

      PageRoot = new Implementation.UI.Grid();
      Content = PageRoot.InternalObject;

      // PageRoot.Margin = new Thickness(5);
      // set project defaults
      // PageRoot.Background = new SolidColorBrush(Global.ProjectManager.Current.ConfigPage.Color);

      // add to docking framework
      Frame frame = new Frame();
      frame.Content = this;
      document = new LayoutDocument();
      document.Title = Global.ProjectManager.Current.Config.Name;
      document.Content = frame;
      Global.AppWindow.AddDocument(document);

    }

    public void Dispose()
    {
      Global.AppWindow.CloseDocument(document);
    }
  }
}
