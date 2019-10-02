using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using IYse;
using Newtonsoft.Json.Linq;

namespace InteractServer.Outputs.MidiStrategy
{
    class MidiCtrlStrategy : IMidiStrategy
    {
        UserControl gui = null;
        public MidiCtrlStrategy(JObject obj)
        {

        }

        public UserControl Gui()
        {
            if (gui == null) gui = new MidiCtrlGui();
            return gui;
        }

        public JObject Save()
        {
            JObject obj = new JObject();
            return obj;
        }

        public static JObject CreateJObject()
        {
            JObject obj = new JObject();
            return obj;
        }

        public int SendInt(IMidiOut output, M_CHANNEL channel, int value)
        {
            output.ControlChange(channel, 0, (byte)value);

            return value;
        }

        public void timerUpdate(IYse.IMidiOut output, int elapsed)
        {

        }
    }
}
