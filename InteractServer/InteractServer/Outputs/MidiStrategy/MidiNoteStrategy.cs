using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace InteractServer.Outputs.MidiStrategy
{
    public class MidiNoteStrategy : IMidiStrategy
    {
        class NoteOff
        {
            public byte note;
            public int period;
            public IYse.M_CHANNEL channel;
        }

        List<NoteOff> noteOffs = new List<NoteOff>();

        public int MinInput { get; set; } = 0;
        public int MaxInput { get; set; } = 127;
        public int MinOutput { get; set; } = 0;
        public int MaxOutput { get; set; } = 127;
        public int Velocity { get; set; } = 64;
        public int Duration { get; set; } = 100;

        UserControl gui;

        public MidiNoteStrategy(JObject obj)
        {
            MinInput = obj.ContainsKey("MinInput") ? Convert.ToInt32(obj["MinInput"]) : 0;
            MinOutput = obj.ContainsKey("MinOutput") ? Convert.ToInt32(obj["MinOutput"]) : 0;

            MaxInput = obj.ContainsKey("MaxInput") ? Convert.ToInt32(obj["MaxInput"]) : 127;
            MaxOutput = obj.ContainsKey("MaxOutput") ? Convert.ToInt32(obj["MaxOutput"]) : 127;

            Velocity = obj.ContainsKey("Velocity") ? Convert.ToInt32(obj["Velocity"]) : 64;
            Duration = obj.ContainsKey("Duration") ? Convert.ToInt32(obj["Duration"]) : 100;
        }

        public UserControl Gui()
        {
            if (gui == null) gui = new MidiNoteGui(this);
            return gui;
        }

        public JObject Save()
        {
            JObject obj = new JObject();
            obj["MinInput" ] = MinInput;
            obj["MinOutput"] = MinOutput;
            obj["MaxInput" ] = MaxInput;
            obj["MaxOutput"] = MaxOutput;
            obj["Velocity" ] = Velocity;
            obj["Duration" ] = Duration;
            return obj;
        }

        public static JObject CreateJObject()
        {
            JObject obj = new JObject();
            obj["MinInput"] = 0;
            obj["MinOutput"] = 0;
            obj["MaxInput"] = 127;
            obj["MaxOutput"] = 127;
            obj["Velocity"] = 64;
            obj["Duration"] = 100;
            return obj;
        }

        public int SendInt(IYse.IMidiOut output, IYse.M_CHANNEL channel, int value)
        {
            byte adjusted = (byte)adjustValue(value);
            output.NoteOn(channel, adjusted, (byte)Velocity);
            if(Duration > 0)
            {
                noteOffs.Add(new NoteOff { note = adjusted, period = Duration, channel = channel });
            }
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
            for(int i = noteOffs.Count - 1; i >= 0; i--)
            {
                NoteOff current = noteOffs[i];
                current.period -= elapsed;
                if(current.period <= 0)
                {
                    output.NoteOn(current.channel, current.note, 0);
                    noteOffs.RemoveAt(i);
                }
            }
        }
    }
}
