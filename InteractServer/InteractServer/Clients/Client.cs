using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OscGuiControl;

namespace InteractServer.Clients
{
	public class Client : INotifyPropertyChanged, OscGuiControl.IPropertyInterface
	{
		private string name;
		public string Name { get => name; set => name = value; }
		public string ReadableName { get => name; } // for property inspector

		private string ip;
		public string IP
		{
			get => ip;
			set {
				ip = value;
				sender = new Network.OscSender(ip);
			}
		}
		public string ReadableIP => ip; // for property inspector

		private string id;
		public string ID { get => id; }

		private int lastSeen = 0;
		public int LastSeen
		{
			get => lastSeen;
			set => lastSeen = IsFake ? 0 : value;
		}

		public bool IsFake { get; set; } = false;

		private Network.OscSender sender = null;
		public Network.OscSender Send => sender;

		public event PropertyChangedEventHandler PropertyChanged;

		static private PropertyCollection properties = null;
		public PropertyCollection Properties => properties;

		static Client()
		{
			properties = new PropertyCollection();
			properties.Add("ReadableName", "Name");
			properties.Add("ReadableIP", "IP Address");
			properties.Add("ID", "Identifier");
		}

		public Client(string name, string ipAddress, string id)
		{
			this.name = name;
			IP = ipAddress;
			this.id = id;
		}

		public override string ToString()
		{
			if (this.name == string.Empty) return this.ReadableIP;
			else return this.name;
		}

		private void NotifyPropertyChanged(String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void ConfirmPresence(String address)
		{
			lastSeen = 0;
			if (ip.Equals(address)) return;

			// ip address has changed! (might be possible because of dhcp lease?)
			IP = address;
		}

		#region Method Queue
		private Queue<MethodInvoker> queue = new Queue<MethodInvoker>();
		public Queue<MethodInvoker> Queue { get => queue; set => queue = value; }
		private bool idle = true;

		public void QueueMethod(MethodInvoker method)
		{
			if (idle)
			{
				method();
				idle = false;
			}
			else
			{
				queue.Enqueue(method);
			}
		}

		// called when a client indicates it has no running taks anymore
		public void GetNextMethod()
		{
			if (queue.Count > 0)
			{
				MethodInvoker method = queue.Dequeue();
				method();
			}
			else
			{
				idle = true;
				Log.Log.Handle.AddEntry("Client " + Name + " is now idle.");
			}
		}

		public void ClearQueue()
		{
			queue.Clear();
			idle = true;
		}
		#endregion Method Queue
	}
}
