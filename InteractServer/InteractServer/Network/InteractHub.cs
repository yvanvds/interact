using InteractServer.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer
{

  public class InteractHub : Hub
  {


    public void SetName(string name)
    {
      Client client = Global.Clients.Get(Context.ConnectionId);
      if (client != null)
      {
        App.Current.Dispatcher.Invoke((Action)delegate
        {
          client.UserName = name;
        });
      }
    }

    public void Acknowledge()
    {
      App.Current.Dispatcher.Invoke((Action)delegate
      {
        Global.Clients.List[Context.ConnectionId]?.ConfirmPresence(Context.Request.Environment["server.RemoteIpAddress"].ToString());
      });
    }

    public void GetNextMethod()
    {
      Client client = Global.Clients.Get(Context.ConnectionId);
      if (client != null)
      {
        client.GetNextMethod();
      }
    }

    public void GetProjectConfig(Guid ID)
    {
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " asks for project configuration.");
      Global.ProjectManager.Current.SendConfigToClient(Context.ConnectionId);
    }

    public void GetScreen(Guid projectID, int screenID)
    {
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " asks for screen " + screenID + ".");

      // TODO: see if ID is equal to current project
      Global.ProjectManager.Current.Screens.Get(screenID).SendToClient(Context.ConnectionId);
    }

    public void GetImage(Guid projectID, int imageID)
    {
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " asks for image " + imageID + ".");
      Global.ProjectManager.Current.Images.Get(imageID).SendToClient(Context.ConnectionId);
    }

    public void GetSoundFile(Guid projectID, int sfID)
    {
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " asks for soundfile " + sfID + ".");
      Global.ProjectManager.Current.SoundFiles.Get(sfID).SendToClient(Context.ConnectionId);
    }

    public void InvokeMethod(string name, params object[] arguments)
    {
      if(JintEngine.Runner.Engine != null)
      {
        Array.Resize<object>(ref arguments, arguments.Length + 1);
        arguments[arguments.Length - 1] = Context.ConnectionId;
        JintEngine.Runner.Engine.InvokeMethod(name, arguments);
      }
    }

    public void Log(string message)
    {
      Global.Log.AddEntry(Global.Clients.Get(Context.ConnectionId).UserName + ": " + message);
    }

    public override Task OnConnected()
    {

      Global.Clients.Add(Context.ConnectionId, new Models.Client
      {
        IpAddress = Context.Request.Environment["server.RemoteIpAddress"].ToString(),
      });
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " connected.");
      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      Global.Log.AddEntry("Client disconnected.");
      Global.Clients.Remove(Context.ConnectionId);
      return base.OnDisconnected(stopCalled);
    }

    public override Task OnReconnected()
    {
      Global.Clients.Add(Context.ConnectionId, new Models.Client
      {
        IpAddress = Context.Request.Environment["server.RemoteIpAddress"].ToString(),
      });
      Global.Log.AddEntry("Client " + Global.Clients.Get(Context.ConnectionId).UserName + " reconnected.");

      // make sure queued methods get sent
      GetNextMethod();

      return base.OnReconnected();
    }
  }
}
