using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Implementation.Network
{
  public class OscReceiver : Interact.Network.OscReceiver
  {
    private Interface.IoscReceiver receiver;
    private Task task;
    private CancellationTokenSource cancellationTokenSource;
    private bool started = false;

    private Dictionary<string, string> Routes = new Dictionary<string, string>();

    public OscReceiver()
    {
      receiver = DependencyService.Get<Interface.IoscReceiver>();
    }

    ~OscReceiver()
    {
      Stop();
    }

    public override void Init(int port)
    {
      receiver.Init(port);
    }

    public override void Register(string address, string callbackFunction)
    {
      Routes[address] = callbackFunction;
    }

    public override void Start()
    {
      if (started) return;
      if(receiver.Start())
      {
        cancellationTokenSource = new CancellationTokenSource();
        task = Task.Run(() => ParseAsync(cancellationTokenSource.Token));
        started = true;
      }
    }

    public override void Stop()
    {
      if (!started) return;
      cancellationTokenSource.Cancel();

      try
      {
        task.Wait();
      }
      catch (AggregateException e)
      {
        foreach (var v in e.InnerExceptions)
        {
          InteractClient.Network.Service.Get().WriteLog("OscReceiver: " + e.Message + " " + v.Message);
        }
      }
      finally
      {
        cancellationTokenSource.Dispose();
      }

      receiver.Stop();
      started = false;
    }

    private async Task ParseAsync(CancellationToken token)
    {
      while(true)
      {
        if(receiver.State == Rug.Osc.OscSocketState.Connected)
        {
          try
          {
            Rug.Osc.OscMessage message;
            while(receiver.TryReceive(out message))
            {
              foreach(var entry in Routes)
              {
                if(message.Address.StartsWith(entry.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                  JintEngine.Engine.Instance.Invoke(entry.Value, message.ToArray());
                }
              }
            }
          } catch(Exception e)
          {
            InteractClient.Network.Service.Get().WriteLog("OscReceiver Error: " + e.Message);
          }
        }

        if(token.IsCancellationRequested)
        {
          token.ThrowIfCancellationRequested();
        }

        // don't run this task at full speed
        await Task.Delay(20);
      }
    }
  }
}
