using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InteractServer.Outputs.MidiStrategy
{
    public class MidiListStrategy : IMidiStrategy
    {
        public enum MessageType
        {
            NoteOn,
            ProgramChange,
            ControlChange,
        }

        public class TimedMessage
        {
            public int Offset { get; set; }
            public IYse.M_CHANNEL Channel { get; set; }
            public MessageType Type { get; set; }
            public byte Value1 { get; set; }
            public byte Value2 { get; set; }

            public TimedMessage GetCopy()
            {
                var result = new TimedMessage
                {
                    Offset = Offset,
                    Channel = Channel,
                    Type = Type,
                    Value1 = Value1,
                    Value2 = Value2,
                };
                return result;
            }
        }
        List<TimedMessage> messageQueue = new List<TimedMessage>();
        public List<TimedMessage> AllMessages { get; set; } = new List<TimedMessage>();

        private readonly object messageLock = new object(); 

        UserControl gui;

        public MidiListStrategy(JObject obj)
        {
            if(obj.ContainsKey("List"))
            {
                var arr = obj["List"].ToArray();
                foreach(JObject line in arr)
                {
                    TimedMessage message = new TimedMessage();
                    message.Offset = Convert.ToInt32(line["Offset"]);
                    message.Channel = (IYse.M_CHANNEL)Enum.Parse(typeof(IYse.M_CHANNEL), line["Channel"].ToString());
                    message.Type = (MessageType)Enum.Parse(typeof(MessageType), line["Type"].ToString());
                    message.Value1 = Convert.ToByte(line["Value1"]);
                    message.Value2 = Convert.ToByte(line["Value2"]);
                    AllMessages.Add(message);
                }
            }
        }

        public UserControl Gui()
        {
            if (gui == null) gui = new MidiListGui(this);
            return gui;
        }

        public JObject Save()
        {
            JObject obj = new JObject();
            var arr = new JArray();
            AllMessages.ForEach(message =>
            {
                JObject line = new JObject();
                line["Offset"] = message.Offset;
                line["Channel"] = message.Channel.ToString();
                line["Type"] = message.Type.ToString();
                line["Value1"] = message.Value1;
                line["Value2"] = message.Value2;
                arr.Add(line);
            });
            obj["List"] = arr;
            return obj;
        }

        public static JObject CreateJObject()
        {
            JObject obj = new JObject();
            return obj;
        }

        public int SendInt(IYse.IMidiOut output, IYse.M_CHANNEL channel, int value)
        {
            lock(messageLock)
            {
                messageQueue.Clear();
                AllMessages.ForEach(message =>
                {
                    messageQueue.Add(message.GetCopy());
                });
            }
            return 1;
        }

        public void timerUpdate(IYse.IMidiOut output, int elapsed)
        {
            lock(messageLock)
            {
                for(int i = messageQueue.Count - 1; i >= 0; i--)
                {
                    TimedMessage message = messageQueue[i];
                    message.Offset -= elapsed;
                    if(message.Offset <= 0)
                    {
                        switch(message.Type)
                        {
                            case MessageType.ControlChange:
                                output.ControlChange(message.Channel, message.Value1, message.Value2);
                                break;
                            case MessageType.NoteOn:
                                output.NoteOn(message.Channel, message.Value1, message.Value2);
                                break;
                            case MessageType.ProgramChange:
                                output.ProgramChange(message.Channel, message.Value1);
                                break;
                        }
                        messageQueue.RemoveAt(i);
                    }
                }
            }
        }
    }
}
