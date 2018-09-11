using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.Groups
{
	public class GroupMember : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public string ID { get; set; }

		public SolidColorBrush color = new SolidColorBrush(Colors.Pink);
		public SolidColorBrush Color => color;

		public Clients.Client handle = null;
		public Clients.Client Handle
		{
			get
			{
				return handle;
			}
			set
			{
				handle = value;
				if(handle == null)
				{
					color = new SolidColorBrush(Colors.Red);
				} else
				{
					color = new SolidColorBrush(Colors.Green);
				}
				NotifyPropertyChanged("Color");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public GroupMember(string name, string id, Clients.Client client = null)
		{
			Name = name;
			ID = id;
			Handle = client;
		}

		public GroupMember(Clients.Client client)
		{
			Name = client.Name;
			ID = client.ID;
			Handle = client;
		}
	}
}
