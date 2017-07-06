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
    private Queue<MethodInvoker> Queue = new Queue<MethodInvoker>();
    private bool Idle = true;

    protected String _UserName;
    protected String _IpAddress;
    protected int _LastSeen = 0;
    protected String _Background = "Green";

    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsSelected { get; set; } = true;

    public void ConfirmPresence(String address)
    {
      LastSeen = 0;
      if (_IpAddress.Equals(address)) return;

      // ip address has changed! (might be possible because of dhcp lease?)
      _IpAddress = address;
    }


    public String UserName
    {
      get
      {
        return _UserName;
      }
      set
      {
        _UserName = value;
        NotifyPropertyChanged("UserName");
      }
    }


    public String IpAddress
    {
      get
      {
        return _IpAddress;
      }
      set
      {
        _IpAddress = value;
        NotifyPropertyChanged("IpAddress");
      }
    }

    public int LastSeen
    {
      get
      {
        return _LastSeen;
      }
      set
      {
        if (_LastSeen != value)
        {
          _LastSeen = value;
          if (_LastSeen == 1) return; // background color does not change
          if (_LastSeen == 3) Background = "red";
          else if (_LastSeen == 2) Background = "orange";
          else Background = "green";
        }
      }
    }


    public String Background
    {
      get
      {
        return _Background;
      }

      set
      {
        _Background = value;
        NotifyPropertyChanged("Background");
      }
    }

    public Queue<MethodInvoker> Queue1 { get => Queue; set => Queue = value; }

    private void NotifyPropertyChanged(String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public void QueueMethod(MethodInvoker method)
    {
      if (Idle)
      {
        method();
        Idle = false;
      }
      else
      {
        Queue.Enqueue(method);
      }
    }

    // called when a client indicate it has no running taks anymore
    public void GetNextMethod()
    {
      if (Queue.Count > 0)
      {
        MethodInvoker method = Queue.Dequeue();
        method();
      }
      else
      {
        Idle = true;
        Global.Log.AddEntry("Client " + UserName + " is now idle.");
      }
    }
  }


}
