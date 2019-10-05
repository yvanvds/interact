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
    public class MidiCtrlStrategy : IMidiStrategy
    {
        UserControl gui = null;

        public int SelectedController { get; set; } = 0;
        public int MinInput { get; set; } = 0;
        public int MaxInput { get; set; } = 127;
        public int MinOutput { get; set; } = 0;
        public int MaxOutput { get; set; } = 127;

        public MidiCtrlStrategy(JObject obj)
        {
            SelectedController = obj.ContainsKey("SelectedController") ? Convert.ToInt32(obj["SelectedController"]) : 0;

            MinInput = obj.ContainsKey("MinInput") ? Convert.ToInt32(obj["MinInput"]) : 0;
            MinOutput = obj.ContainsKey("MinOutput") ? Convert.ToInt32(obj["MinOutput"]) : 0;

            MaxInput = obj.ContainsKey("MaxInput") ? Convert.ToInt32(obj["MaxInput"]) : 127;
            MaxOutput = obj.ContainsKey("MaxOutput") ? Convert.ToInt32(obj["MaxOutput"]) : 127;
        }

        public UserControl Gui()
        {
            if (gui == null) gui = new MidiCtrlGui(this);
            return gui;
        }

        public JObject Save()
        {
            JObject obj = new JObject();
            obj["SelectedController"] = SelectedController;
            obj["MinInput"] = MinInput;
            obj["MinOutput"] = MinOutput;
            obj["MaxInput"] = MaxInput;
            obj["MaxOutput"] = MaxOutput;
            return obj;
        }

        public static JObject CreateJObject()
        {
            JObject obj = new JObject();
            obj["SelectedController"] = 0;
            obj["MinInput"] = 0;
            obj["MinOutput"] = 0;
            obj["MaxInput"] = 127;
            obj["MaxOutput"] = 127;
            return obj;
        }

        public int SendInt(IMidiOut output, M_CHANNEL channel, int value)
        {
            byte adjusted = (byte)adjustValue(value);
            output.ControlChange(channel, (byte)SelectedController, adjusted);

            return adjusted;
        }

        private int adjustValue(int value)
        {
            float f = value - MinInput;
            if (f < 0 || f > MaxInput) return 0;
            f /= MaxInput - MinInput;
            f *= MaxOutput - MinOutput;
            f += MinOutput;
            return (int)f;
        }

        public void timerUpdate(IYse.IMidiOut output, int elapsed)
        {

        }
    }
}
