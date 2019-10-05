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
    public enum ProgramOptions
    {
        INCOMING,
        FIXED,
    }

    public class MidiProgramStrategy : IMidiStrategy
    {
        UserControl gui;
        public int FixedValue { get; set; } = 64;

        public ProgramOptions ProgramOptions { get; set; } = ProgramOptions.INCOMING;

        public MidiProgramStrategy(JObject obj)
        {
            ProgramOptions = obj.ContainsKey("ProgramOption") 
                ? (ProgramOptions)Enum.Parse(typeof(ProgramOptions), (string)obj["ProgramOption"]) 
                : ProgramOptions.INCOMING;
            FixedValue = obj.ContainsKey("FixedValue") ? Convert.ToInt32(obj["FixedValue"]) : 64;
        }

        public UserControl Gui()
        {
            if (gui == null) gui = new MidiProgramGui(this);
            return gui;
        }

        public JObject Save()
        {
            JObject obj = new JObject();
            obj["ProgramOption"] = ProgramOptions.ToString();
            obj["FixedValue"] = FixedValue;
            return obj;
        }

        public static JObject CreateJObject()
        {
            JObject obj = new JObject();
            obj["ProgramOption"] = ProgramOptions.INCOMING.ToString();
            obj["FixedValue"] = 64;
            return obj;
        }

        public int SendInt(IMidiOut output, M_CHANNEL channel, int value)
        {
            switch(ProgramOptions)
            {
                default:
                    output.ProgramChange(channel, (byte)value);
                    return value;
                case ProgramOptions.FIXED:
                    output.ProgramChange(channel, (byte)FixedValue);
                    return FixedValue;
            }
        }

        public void timerUpdate(IYse.IMidiOut output, int elapsed)
        {

        }
    }
}
