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
    /// Interaction logic for NewRouterDialog.xaml
    /// </summary>
    public partial class NewOutputDialog : MetroWindow
    {
        public string OutputName { get; set; }
        public string OutputDescription { get; set; }

        public NewOutputDialog()
        {
            InitializeComponent();
            //BContinue.IsEnabled = false;
            LInvalid.Visibility = Visibility.Hidden;

        }

        private void BContinue_Click(object sender, RoutedEventArgs e)
        {
            if (!validModel()) return;
            DialogResult = true;
            Close();
        }

        private bool validModel()
        {
            OutputName = TBOutputName.Text.Trim();
            OutputDescription = TOutputDescription.Text.Trim();

            if (OutputName.Length == 0)
            {
                LInvalid.Visibility = Visibility.Visible;
                return false;
            } else
            {
                LInvalid.Visibility = Visibility.Hidden;
                return true;
            }
        }

        private void TBOutputName_TextChanged(object sender, TextChangedEventArgs e)
        {
            BContinue.IsEnabled = validModel();
        }

        private void TBOutputName_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Return && BContinue.IsEnabled)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
