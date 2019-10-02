using InteractServer.Outputs;
using Newtonsoft.Json.Linq;
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
using System.Windows.Threading;

namespace InteractServer.Controls
{
    /// <summary>
    /// Interaction logic for RouterList.xaml
    /// </summary>
    /// 


    public partial class OutputList : UserControl
    {
        private bool needsSaving = false;
        public bool NeedsSaving => true; // TODO: optimize later

        private DispatcherTimer GuiTimer = null;

        private OscTree.Tree tree = null;

        List<Output> outputs = new List<Output>();
        public List<Output> Outputs => outputs;

        public OutputList(string name, string ID)
        {
            InitializeComponent();
            Name = name;

            tree = new OscTree.Tree(new OscTree.Address(name, ID));
            Osc.Tree.ServerRouters.Add(tree);

            GuiTimer = new DispatcherTimer();
            GuiTimer.Tick += GuiTimer_Tick;
            GuiTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            previousTickCount = Environment.TickCount;
            GuiTimer.Start();

            Panel.ItemsSource = Outputs;
        }

        int previousTickCount = 0;
        private void GuiTimer_Tick(object sender, EventArgs e)
        {
            int newTickCount = Environment.TickCount;
            int elapsed = newTickCount - previousTickCount;
            Outputs.ForEach(output =>
            {
                output.TimerUpdate(elapsed);
            });
            previousTickCount = newTickCount;
        }

        private void AddOutputButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.NewOutputDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                string ID = shortid.ShortId.Generate(false, false);
                var obj = Output.CreateJObject(dialog.OutputName, ID, dialog.OutputDescription, OutputType.MIDI);
                outputs.Add(new Output(obj, tree, this));
                Panel.Items.Refresh();
                needsSaving = true;
            }
        }

        public JObject Save()
        {
            var obj = new JObject();
            foreach(Output output in Outputs)
            {
                obj[output.ID] = output.Save();
            }
            return obj;
        }

        public void Load(JObject content)
        {
            foreach (var obj in content.Values())
            {
                Outputs.Add(new Output(obj as JObject, tree, this));
            }
        }


    }
}
