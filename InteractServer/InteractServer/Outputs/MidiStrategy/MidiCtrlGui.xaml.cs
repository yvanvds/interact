﻿using System;
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
    /// Interaction logic for MidiCtrlGui.xaml
    /// </summary>
    public partial class MidiCtrlGui : UserControl
    {
        MidiCtrlStrategy parent;

        public MidiCtrlGui(MidiCtrlStrategy data)
        {
            InitializeComponent();
            parent = data;
            this.DataContext = data;
            ControlSelector.ItemsSource = Utils.Midi.ControllerNames;
            ControlSelector.SelectedIndex = data.SelectedController;
        }

        private void ControlSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parent.SelectedController = ControlSelector.SelectedIndex;
        }
    }
}
