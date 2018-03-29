using InteractClient.Data;
using Jint.Runtime.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.JintEngine
{
    class Engine
    {
        public static Engine Instance = new Engine();
        public static SemaphoreSlim ConstructionLock;

        private bool active = false;
        public bool Active { get => active; }

        private ContentPage activePage;

        private bool ScreenNeedsToStart = false;
        private int screenToStart;

        // Jint Engine
        private Jint.Engine jEngine = null;
        private Implementation.Values Values = new Implementation.Values();
        private Implementation.Network.Server Server = new Implementation.Network.Server();
        private Implementation.Device.Sensors Sensors;
        private Implementation.Device.Arduino Arduino;

        public Jint.Engine Jint()
        {
            if (jEngine == null)
            {
                jEngine = new Jint.Engine();
                jEngine.SetValue("Root", (activePage as ModelPage).PageRoot);
                jEngine.SetValue("Values", Values);

                if (Sensors == null)
                {
                    try
                    {
                        //Sensors = new Implementation.Device.Sensors();
                        //jEngine.SetValue("Sensors", Sensors);
                    }
                    catch (Exception e)
                    {
                        Network.Signaler.Get().WriteLog("Can't setup sensors:" + e.Message);
                    }
                }

                if (Arduino == null)
                {
                    Arduino = new Implementation.Device.Arduino();
                }

                if (Arduino.IsImplemented())
                {
                    Arduino.Start();
                    jEngine.SetValue("Arduino", Arduino);
                }

                // UI
                jEngine.SetValue("Button", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Button)));
                jEngine.SetValue("Title", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Title)));
                jEngine.SetValue("Text", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Text)));
                jEngine.SetValue("Slider", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Slider)));
                jEngine.SetValue("Image", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Image)));
                jEngine.SetValue("Grid", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Grid)));


                jEngine.SetValue("Color", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.UI.Color)));

                // Network
                jEngine.SetValue("Server", Server);
                jEngine.SetValue("Clients", Project.Current.Clients);
                jEngine.SetValue("OscSender", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.Network.OscSender)));
                jEngine.SetValue("OscReceiver", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.Network.OscReceiver)));
                jEngine.SetValue("Timer", TypeReference.CreateTypeReference(jEngine, typeof(Implementation.Logic.Timer)));

                jEngine.SetValue("PinMode", TypeReference.CreateTypeReference(jEngine, typeof(Interact.Device.Arduino.PinMode)));
                jEngine.SetValue("PinState", TypeReference.CreateTypeReference(jEngine, typeof(Interact.Device.Arduino.PinState)));
            }
            return jEngine;
        }

        public void StartScreen(int ID)
        {
            if (ConstructionLock == null) ConstructionLock = new SemaphoreSlim(1);
            ConstructionLock.Wait(5000);

            Arduino?.AllowJintOutput(false);
            Arduino?.RemoveSignalHandlers();

            if (activePage is ConnectedPage)
            {
                ScreenNeedsToStart = true;
                screenToStart = ID;
                ConnectedPage page = activePage as ConnectedPage;
                Device.BeginInvokeOnMainThread(() => page.PushModelPage());
                InteractClient.Network.Signaler.Get().GetNextMethod();

            }
            else if (activePage is ModelPage)
            {

                screenToStart = ID;
                ModelPage p = activePage as ModelPage;
                string screenName = Data.Project.Current.GetScreen(ID).Name;
                Device.BeginInvokeOnMainThread(() => p.StartScript(screenName));
            }
        }

        public void StopScreen()
        {
            if (activePage is ModelPage)
            {
                active = false;
                ModelPage p = activePage as ModelPage;
                Device.BeginInvokeOnMainThread(() => p.Pop());
                jEngine = null;
                Arduino?.RemoveSignalHandlers();
            }
            InteractClient.Network.Signaler.Get().GetNextMethod();
        }

        public void StopRunningProject()
        {
            active = false;
            Sensors?.Stop();
            Arduino?.Stop();
            StopScreen();
            Network.Signaler.Get().WriteLog("Engine->StopRunningProject: Done.");
        }

        public void SetActivePage(ContentPage page)
        {
            activePage = page;
            if (page is ModelPage && ScreenNeedsToStart)
            {
                StartScript(screenToStart);
                ScreenNeedsToStart = false;
            }
        }

        public void StartScript(int ID)
        {
            Screen screen = Project.Current.GetScreen(ID);

            if (screen == null)
            {
                InteractClient.Network.Signaler.Get().WriteLog("Engine->StartScript: screen " + ID + " not found.");
                ConstructionLock.Release();
                return;
            }
            try
            {

                Jint().Execute(screen.screenContent.Content);
                Jint().Invoke("Init");
                InteractClient.Network.Signaler.Get().WriteLog("Engine->StartScript: screen " + ID + " is now running.");
            }
            catch (Jint.Parser.ParserException e)
            {
                InteractClient.Network.Signaler.Get().WriteErrorLog(e.Index, e.LineNumber, e.Message, ID);
                ConstructionLock.Release();
                return;
            }
            catch (Jint.Runtime.JavaScriptException e)
            {
                InteractClient.Network.Signaler.Get().WriteErrorLog(0, e.LineNumber, e.Message, ID);
                ConstructionLock.Release();
                return;
            }
            catch (Exception e)
            {
                InteractClient.Network.Signaler.Get().WriteLog("Engine->StartScript: Error on screen " + ID + ":" + e.Message);
                ConstructionLock.Release();
                return;
            }

            ConstructionLock.Release();
            Arduino?.AllowJintOutput(true);
            active = true;
        }

        public void Invoke(String functionName, params object[] arguments)
        {
            ConstructionLock.Wait(5000);
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Jint().Invoke(functionName, arguments);
                }
                catch (Jint.Parser.ParserException e)
                {
                    InteractClient.Network.Signaler.Get().WriteErrorLog(e.Index, e.LineNumber, e.Message, screenToStart);
                }
                catch (Jint.Runtime.JavaScriptException e)
                {
                    InteractClient.Network.Signaler.Get().WriteErrorLog(0, e.LineNumber, e.Message, screenToStart);
                }
                catch (Exception e)
                {
                    InteractClient.Network.Signaler.Get().WriteLog("Engine->Invoke on function " + functionName + ": " + e.Message);
                }
            });
            ConstructionLock.Release();
        }

        public void SetScreenMessage(string message, bool animate)
        {
            if (activePage == null) return;
            if (activePage is ModelPage) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                (activePage as ConnectedPage)?.SetActivity(message, animate);
            });
        }
    }
}
