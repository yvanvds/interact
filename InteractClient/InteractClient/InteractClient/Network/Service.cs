using Acr.Settings;
using InteractClient.JintEngine;
using Microsoft.AspNet.SignalR.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Network
{
    public class Signaler
    {
        private static Signaler instance = null;
        private HubConnection connection;
        private IHubProxy proxy;
        public IHubProxy Proxy { get => proxy; }
        public ServerList ConnectedServer = null;
        public bool Connected { get; set; } = false;

        public static Signaler Get()
        {
            if (instance == null)
            {
                instance = new Signaler();
            }
            return instance;
        }

        public Signaler()
        {
            Connected = false;
        }

        public async Task ConnectAsync(ServerList server)
        {
            if (Connected)
            {
                Disconnect();
            }
            Connected = true;
            ConnectedServer = server;
            connection = new HubConnection("http://" + server.Address + ":" + Constants.TcpPort + "/signalr");
            connection.Closed += Disconnect;
            proxy = connection.CreateHubProxy("SignalReceiver");
            AddMethods();

            try
            {
                await connection.Start();
                Global.UpdatePage(true);
                await proxy.Invoke("SetName", Settings.Current.Get<String>("UserName"));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Connection failed: " + e.Message);
                Connected = false;
                Global.UpdatePage(false);
            }
        }

        public void Disconnect()
        {
            ConnectedServer = null;
            Connected = false;

            if (connection != null)
            {
                try
                {
                    connection.Stop();
                    Debug.WriteLine("Connection closed");
                }
                catch (NullReferenceException) { }
            }
        }

        public void GetProjectConfig(Guid ID)
        {
            proxy?.Invoke("GetProjectConfig", ID);
        }


        public void GetNextMethod()
        {
            proxy?.Invoke("GetNextMethod");
        }

        public void GetScreen(Guid projectID, int screenID)
        {
            proxy?.Invoke("GetScreen", projectID, screenID);
        }

        public void GetImage(Guid projectID, int imageID)
        {
            proxy?.Invoke("GetImage", projectID, imageID);
        }

        public void GetSoundFile(Guid projectID, int sfID)
        {
            proxy?.Invoke("GetSoundFile", projectID, sfID);
        }

        public void WriteLog(string message)
        {
            proxy?.Invoke("Log", message);
        }

        public void WriteErrorLog(int index, int lineNumber, string message, int resourceID)
        {
            proxy?.Invoke("ErrorLog", index, lineNumber, message, resourceID);
        }

        public void InvokeMethod(string method, params object[] arguments)
        {
            proxy?.Invoke("InvokeMethod", method, arguments);
        }

        public void InvokeMethod(string clientID, string method, params object[] arguments)
        {
            proxy?.Invoke("InvokeMethod", clientID, method, arguments);
        }

        public void StartScreen(string clientID, int screenID)
        {
            proxy?.Invoke("StartScreen", clientID, screenID);
        }

        private void AddMethods()
        {
            proxy.On("RequestAcknowledge", () =>
                proxy.Invoke("Acknowledge")
            );

            proxy.On("SetCurrentProject", (Guid ID, int Version) =>
                {
                    Data.Project.SetCurrent(ID, Version);
                    Engine.Instance.SetScreenMessage("Loading Project...", true);
                }
            );

            proxy.On("SetProjectConfig", (Guid ID, Dictionary<string, string> values) =>
                Data.Project.List[ID]?.SetConfig(values)
            );

            proxy.On("SetScreenVersion", (Guid projectID, int screenID, int Version) =>
                {
                    Data.Project.List[projectID]?.SetScreenVersion(projectID, screenID, Version);
                    Engine.Instance.SetScreenMessage("Loading Screens...", true);
                }
            );

            proxy.On("StopCurrentScreen", () =>
                {
                    Engine.Instance.StopScreen();
                    Engine.Instance.SetScreenMessage("Screen Stopped.", false);
                }
            );

            proxy.On("StopRunningProject", () =>
            {
                Engine.Instance.StopRunningProject();
                WriteLog("Stopping Current Project.");
            });

            proxy.On("StartScreen", (int ID) =>
                Engine.Instance.StartScreen(ID)
            );

            proxy.On("UpdateScreen", (Guid projectID, int ID, string Content) =>
                Data.Project.List[projectID]?.UpdateScreen(ID, Content)
            );

            proxy.On("SetImageversion", (Guid projectID, int imageID, int Version) =>
            {
                Data.Project.List[projectID]?.SetImageVersion(projectID, imageID, Version);
                Engine.Instance.SetScreenMessage("Loading Images...", true);
            });

            proxy.On("UpdateImage", (Guid projectID, int ID, string Content) =>
            {
                Data.Project.List[projectID]?.UpdateImage(ID, Content);
            });

            proxy.On("SetSoundFileVersion", (Guid projectID, int sfID, int Version) =>
            {
                Data.Project.List[projectID]?.SetSoundFileVersion(projectID, sfID, Version);
                Engine.Instance.SetScreenMessage("Loading Sounds...", true);
            });

            proxy.On("UpdateSoundFile", (Guid projectID, int ID, string Content) =>
            {
                Data.Project.List[projectID]?.UpdateSoundFile(ID, Content);
            });

            proxy.On("AddClient", (string IP, string ID, string Name, bool local) =>
            {
                Data.Project.Current.Clients.Add(IP, ID, Name, local);
                GetNextMethod();
            });

            proxy.On("InvokeMethod", (string MethodName, object[] arguments) =>
             {
                 Engine.Instance.Invoke(MethodName, arguments);
             });

            proxy.On("CloseConnection", () =>
            {
                Disconnect();
            });
        }
    }
}
