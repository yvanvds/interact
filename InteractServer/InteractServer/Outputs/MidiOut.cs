using InteractServer.Outputs.MidiStrategy;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Outputs
{
    public class MidiOut : IOut, INotifyPropertyChanged

    {
        IYse.IMidiOut output;
        MidiOutGui gui = null;
        

        private string message;
        public string Message => message;

        private byte port = 0;
        public byte Port
        {
            get => port;
            set
            {
                port = value;
                setPort();
            }
        }

        private byte channel = 0;
        public byte Channel
        {
            get => channel;
            set
            {
                channel = value;
            }
        }


        private MidiOutputType currentMidiType = MidiOutputType.NOTE;
        public MidiOutputType CurrentMidiType {
            get => currentMidiType;
            set
            {
                if (currentMidiType != value)
                {
                    currentMidiType = value;
                    midiStrategy = Factory.CreateStrategy(currentMidiType, null);
                    updateProperty("DetailsGui");
                }
            }
        }

        IMidiStrategy midiStrategy = null;

        Action<string> UpdateGui = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserControl DetailsGui
        {
            get
            {
                if (midiStrategy == null) return null;
                return midiStrategy.Gui();
            }
        }

        public MidiOut(JObject obj)
        {
            currentMidiType = obj.ContainsKey("Type") ? (MidiOutputType)Enum.Parse(typeof(MidiOutputType), (string)obj["Type"]) : MidiOutputType.NOTE;
            port = obj.ContainsKey("Port") ? Convert.ToByte(obj["Port"]) : (byte)0;
            channel = obj.ContainsKey("Channel") ? Convert.ToByte(obj["Channel"]) : (byte)0;
            output = Yse.Yse.Handle.Interface.NewMidiOut();
            setPort();

            midiStrategy = MidiStrategy.Factory.CreateStrategy(currentMidiType, obj.ContainsKey("Strategy") ? (JObject)obj["Strategy"] : null);
        }

        private void setPort()
        {
            output.Create(Port);
        }

        public void SendInt(int value)
        {
            int modifiedValue = midiStrategy.SendInt(output, (IYse.M_CHANNEL)Channel, value);
            message = value.ToString() + " => " + modifiedValue;
            UpdateGui?.Invoke("Message");
        }

        public void SendIntPair(int a, int b)
        {
            throw new NotImplementedException();
        }

        public UserControl Gui()
        {
            if (gui == null)
            {
                gui = new MidiOutGui(this);
            }
            return gui;
        }

        public void OnPropertyUpdate(Action<string> f)
        {
            UpdateGui = f;
        }

        public void TimerUpdate(int elapsed)
        {
            if (midiStrategy != null)
            {
                midiStrategy.timerUpdate(output, elapsed);
            }
        }

        public JObject Save()
        {
            var obj = new JObject();
            obj["Type"] = CurrentMidiType.ToString();
            obj["Port"] = Port;
            obj["Channel"] = Channel;
            if (midiStrategy != null)
            {
                obj["Strategy"] = midiStrategy.Save();
            }
            return obj;
        }

        public static JObject CreateJObject()
        {
            var obj = new JObject();
            obj["Type"] = MidiOutputType.NOTE.ToString();
            obj["Port"] = 0;
            obj["Channel"] = 0;
            return obj;
        }

        void updateProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
