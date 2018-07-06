using Shared;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InteractServer.Models
{
  public class Client : INotifyPropertyChanged
  {
    private Queue<MethodInvoker> queue = new Queue<MethodInvoker>();
    private bool idle = true;

    protected String name;
    protected String ipAddress;
    protected int lastSeen = 0;
    protected String background = "Green";

    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsSelected { get; set; } = true;

		private Network.Sender sender = null;
		public Network.Sender Send => sender;
		
		public Client(string name, string ipAddress)
		{
			UserName = name;
			IpAddress = ipAddress;
		}

    public void ConfirmPresence(String address)
    {
      LastSeen = 0;
      if (ipAddress.Equals(address)) return;

      // ip address has changed! (might be possible because of dhcp lease?)
      IpAddress = address;
    }


    public String UserName
    {
      get
      {
        return name;
      }
      set
      {
        name = value;
        NotifyPropertyChanged("UserName");
      }
    }


    public String IpAddress
    {
      get
      {
        return ipAddress;
      }
      set
      {
				if(!value.Equals(ipAddress))
				{
					ipAddress = value;
					sender = new Network.Sender(ipAddress);
					NotifyPropertyChanged("IpAddress");
				}
      }
    }

    public int LastSeen
    {
      get
      {
        return lastSeen;
      }
      set
      {
        if (lastSeen != value)
        {
          lastSeen = value;
          if (lastSeen == 1) return; // background color does not change
          if (lastSeen == 3) Background = "red";
          else if (lastSeen == 2) Background = "orange";
          else Background = "green";
        }
      }
    }


    public String Background
    {
      get
      {
        return background;
      }

      set
      {
        background = value;
        NotifyPropertyChanged("Background");
      }
    }

    public Queue<MethodInvoker> Queue { get => queue; set => queue = value; }

    private void NotifyPropertyChanged(String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

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
        Global.Log.AddEntry("Client " + UserName + " is now idle.");
      }
    }
  }


}
