using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Outputs.MidiStrategy
{
    static class Factory
    {
        public static IMidiStrategy CreateStrategy(MidiOutputType type, JObject details)
        {
            switch(type)
            {   
                case MidiOutputType.CTRL:
                    {
                        JObject content = details != null ? details : MidiCtrlStrategy.CreateJObject();
                        return new MidiCtrlStrategy(content);
                    }

                case MidiOutputType.PROGRAM:
                    {
                        JObject content = details != null ? details : MidiProgramStrategy.CreateJObject();
                        return new MidiProgramStrategy(content);
                    }

                default:
                    {
                        JObject content = details != null ? details : MidiNoteStrategy.CreateJObject();
                        return new MidiNoteStrategy(content);
                    }
            }
        }
    }
}
