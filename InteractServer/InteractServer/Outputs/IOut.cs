using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Outputs
{
    interface IOut
    {
        void SendInt(int value);

        void SendIntPair(int a, int b);

        UserControl Gui();

        string Message { get; }

        void OnPropertyUpdate(Action<string> f);

        void TimerUpdate(int elapsed);

        JObject Save();
    }
}
