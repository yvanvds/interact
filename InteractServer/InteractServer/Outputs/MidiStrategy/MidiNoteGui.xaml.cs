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

namespace InteractServer.Outputs.MidiStrategy
{
    /// <summary>
    /// Interaction logic for MidiNoteGui.xaml
    /// </summary>
    public partial class MidiNoteGui : UserControl
    {
        public MidiNoteGui(MidiNoteStrategy data)
        {
            InitializeComponent();
            this.DataContext = data;
        }
    }
}
