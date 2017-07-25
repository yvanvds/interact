using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractServer.Implementation.Network
{
  public class OscReceiver : Interact.Network.OscReceiver
  {
    private Rug.Osc.OscReceiver receiver;
    private Task task;
    private CancellationTokenSource cancellationTokenSource;
    private bool started = false;

    private Dictionary<string, string> Routes = new Dictionary<string, string>();

    ~OscReceiver()
    {
      Stop();
    }

    public override void Init(int port)
    {
      if (receiver == null) receiver = new Rug.Osc.OscReceiver(port);
      else Global.Log.AddEntry("OscReceiver was already initialized.");
    }

    public override void Register(string address, string callbackFunction)
    {
      Routes[address] = callbackFunction;
    }

    public override void Start()
    {
      if (started) return;
      if (receiver == null) Global.Log.AddEntry("OscReceiver must call Init before Start");
      else
      {
        receiver.Connect();
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
          Global.Log.AddEntry("OscReceiver: " + e.Message + " " + v.Message);
        }
      }
      finally
      {
        cancellationTokenSource.Dispose();
      }

      receiver?.Close();
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
            Rug.Osc.OscPacket packet;
            while(receiver.TryReceive(out packet))
            {
              Rug.Osc.OscMessage message = Rug.Osc.OscMessage.Parse(packet.ToString());

              foreach(var entry in Routes)
              {
                if(message.Address.StartsWith(entry.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                  JintEngine.Runner.Engine?.InvokeMethod(entry.Value, message.ToArray());
                }
              }
            }
          } catch(Exception e)
          {
            Global.Log.AddEntry("OscReceiver Error: " + e.Message);
          }
        }

        if(token.IsCancellationRequested)
        {
          token.ThrowIfCancellationRequested();
        }

        // don't run this task at full speed
        await Task.Delay(1);
      }
    }
  }
}
