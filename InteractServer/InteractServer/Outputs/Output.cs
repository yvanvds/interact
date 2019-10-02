using InteractServer.Controls;
using InteractServer.Project;
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
    public class Output : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private string id;
        public string ID => id;
        public UserControl Gui => output.Gui();
        public string Message => output.Message;

        OutputType type;

        public OscTree.Object osc;
        private OutputList parent;

        IOut output = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Output(JObject obj, OscTree.Tree oscParent, OutputList parent)
        {
            id = obj.ContainsKey("ID") ? (string)obj["ID"] : String.Empty;
            Name = obj.ContainsKey("Name") ? (string)obj["Name"] : string.Empty;
            Description = obj.ContainsKey("Description") ? (string)obj["Description"] : string.Empty;
            type = obj.ContainsKey("Type") ? StringToOutputtype((string)obj["Type"]) : OutputType.MIDI;
            var details = obj.ContainsKey("Details") ? obj["Details"] : null;
            setOutput((JObject)details);

            this.parent = parent;

            osc = new OscTree.Object(new OscTree.Address(Name, id), typeof(int));
            oscParent.Add(osc);
            osc.Endpoints.Add(new OscTree.Endpoint("Send", (args) =>
            {
                if (output == null) return;
                if(args.Count() > 0)
                {
                    output.SendInt(Convert.ToInt32(args[0]));
                }
            }, typeof(int)));
        }

        void setOutput(JObject details)
        {
            switch(type)
            {
                case OutputType.MIDI:
                    var content = details != null ? details : MidiOut.CreateJObject();
                    output = new MidiOut(content);
                    break;
                case OutputType.OSC: output = null; break;
            }

            if(output != null)
            {
                output.OnPropertyUpdate(UpdateProperty);
            }
        }
        
        public JObject Save()
        {
            var obj = new JObject();
            obj["ID"] = ID;
            obj["Name"] = Name;
            obj["Description"] = Description;
            obj["Type"] = type.ToString();
            if(output != null)
            {
                obj["Details"] = output.Save();
            }
            return obj;
        }

        public static JObject CreateJObject(string name, string id, string description, OutputType type)
        {
            var obj = new JObject();
            obj["ID"] = id;
            obj["Name"] = name;
            obj["Description"] = description;
            obj["Type"] = type.ToString();
            return obj;
        }

        static OutputType StringToOutputtype(string type)
        {
            switch(type)
            {
                case "OSC": return OutputType.OSC;

                default: return OutputType.MIDI;
            }
        }

        
        public void UpdateProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TimerUpdate(int elapsed)
        {
            output.TimerUpdate(elapsed);
        }
    }
}
