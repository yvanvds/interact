using Owin;
using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Shared;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InteractServer.Network
{
    public class Service
    {
        public IDisposable SignalR { get; set; }
        public IHubConnectionContext<dynamic> Clients { get; set; }


        public void Start()
        {
            try
            {
                SignalR = WebApp.Start<Startup>("http://*:" + Constants.TcpPort);
            } catch
            {
                // this probably means a server is already running
                Global.Log.AddEntry("Unable to start server at port " + Constants.TcpPort);
                return;
            }
            Clients = GlobalHost.ConnectionManager.GetHubContext<InteractHub>().Clients;
            Global.Log.AddEntry("Server started at port " + Constants.TcpPort);
        }

        public void Stop()
        {
            SignalR.Dispose();
        }

        public void RequestAcknowledge()
        {
            Clients?.All.requestAcknowledge();
        }

        public void StopCurrentScreen()
        {
            Clients?.All.StopCurrentScreen();
        }

        public void SetCurrentProject(string clientID, Guid ID, int Version)
        {
            Clients?.Client(clientID).SetCurrentProject(ID, Version);
            Global.Log.AddEntry("Project " + ID + " made current on client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendProjectConfig(string clientID, Guid ProjectID, Dictionary<string,string> values)
        {
            Clients.Client(clientID).SetProjectConfig(ProjectID, values);
            Global.Log.AddEntry("Project configuration sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendScreenVersion(string clientID, Guid ProjectID, int ID, int Version)
        {
            Clients.Client(clientID).SetScreenVersion(ProjectID, ID, Version);
            Global.Log.AddEntry("Screen " + ID + " version sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendScreen(List<string> clients, int screenID, string screenData)
        {
            Clients.Clients(clients).UpdateScreen(screenID, screenData);

        }

        public void SendScreen(Guid projectID, string clientID, int screenID, string screenData)
        {
            Clients.Client(clientID).UpdateScreen(projectID, screenID, screenData);
            Global.Log.AddEntry("Screen " + screenID + " sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void StartScreen(string clientID, int screenID)
        {
            Clients.Client(clientID).StartScreen(screenID);
            Global.Log.AddEntry("Screen " + screenID + " started on client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendImageVersion(string clientID, Guid ProjectID, int ID, int Version)
        {
            Clients.Client(clientID).SetImageVersion(ProjectID, ID, Version);
            Global.Log.AddEntry("Image " + ID + " version sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendImage(List<string> clients, int imageID, string imageData)
        {
            Clients.Clients(clients).UpdateImage(imageID, imageData);
        }

        public void SendImage(Guid projectID, string clientID, int imageID, string imageData)
        {
            Clients.Client(clientID).UpdateImage(projectID, imageID, imageData);
            Global.Log.AddEntry("Image " + imageID + " sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendSoundFileVersion(string clientID, Guid ProjectID, int ID, int Version)
        {
            Clients.Client(clientID).SetSoundFileVersion(ProjectID, ID, Version);
            Global.Log.AddEntry("Soundfile " + ID + " version sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }

        public void SendSoundFile(List<string> clients, int sfID, string sfData)
        {
            Clients.Clients(clients).UpdateSoundFile(sfID, sfData);
        }

        public void SendSoundFile(Guid projectID, string clientID, int sfID, string sfData)
        {
            Clients.Client(clientID).UpdateSoundFile(projectID, sfID, sfData);
            Global.Log.AddEntry("Soundfile " + sfID + " sent to client " + Global.Clients.Get(clientID).UserName + ".");
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}

