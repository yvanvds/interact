using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Outputs.MidiStrategy
{
    interface IMidiStrategy
    {
        int SendInt(IYse.IMidiOut output, IYse.M_CHANNEL channel, int value);

        UserControl Gui();

        JObject Save();

        void timerUpdate(IYse.IMidiOut output, int elapsed);
    }
}
