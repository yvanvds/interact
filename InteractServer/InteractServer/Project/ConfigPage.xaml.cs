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

namespace InteractServer.Project
{
    /// <summary>
    /// Interaction logic for ProjectConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        public ConfigPage()
        {
            InitializeComponent();
        }

        public void LinkProject(Project project)
        {
            project.ConfigButton.Link = ExampleButton;
            project.ConfigPage.Link = Page;
            project.ConfigText.Link = ExampleText;
            project.ConfigTitle.Link = ExampleTitle;
        }

        private void ExampleButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Global.PropertiesPage.SetSelected(Global.ProjectManager.Current.ConfigButton);
        }

        private void BackgroundButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Global.PropertiesPage.SetSelected(Global.ProjectManager.Current.ConfigPage);
        }

        private void TitleButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Global.PropertiesPage.SetSelected(Global.ProjectManager.Current.ConfigTitle);
        }

        private void TextButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Global.PropertiesPage.SetSelected(Global.ProjectManager.Current.ConfigText);
        }

        private void ConfigButton_GotFocus(object sender, RoutedEventArgs e)
        {
            Global.PropertiesPage.SetSelected(Global.ProjectManager.Current.Config);
        }
    }
}
