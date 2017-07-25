using Interact;
using Interact.Utility;
using InteractClient.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.Implementation.Device
{
  public class Sensor : Interact.Device.Sensor
  {
    private SensorValue _CurrentValue;
    private float _ScaleMin;
    private float _ScaleMax;

    public override SensorValue CurrentValue => _CurrentValue;

    private IoscSender sender = null;
    private string Address;
    private bool initDone = false;
    private SensorType Type;

    public Sensor(SensorType Type)
    {
      this.Type = Type;
      switch (Type)
      {
        case SensorType.Compass:
          {
            ScaleMin = 0;
            ScaleMax = 360;
            break;
          }
      }
    }

    

    public override float ScaleMin { get => _ScaleMin; set => _ScaleMin = value; }
    public override float ScaleMax { get => _ScaleMax; set => _ScaleMax = value; }

    public override void Route(string IPAddress, int port, string OscAddress)
    {
      initDone = false;
      sender = DependencyService.Get<IoscSender>();
      sender.Init(IPAddress, port);
      this.Address = OscAddress;
      initDone = true;
    }



    public override void Send(SensorValue value)
    {
      _CurrentValue = Scale(value);
      if (!initDone) return;
      if (value is SensorVector)
      {
        SensorVector vector = _CurrentValue as SensorVector;
        sender?.Send(Address, vector.X, vector.Y, vector.Z);
      }
      else
      {
        sender?.Send(Address, _CurrentValue.Value);
      }
    }

    public override void Start(int delay) // valid delays are 0, 20, 60 and 200 ms 
    {
      Sensors.Current.Start(Type, (SensorDelay)delay);
    }

    public override void Stop()
    {
      Sensors.Current.Stop(Type);
    }

    private SensorValue Scale(SensorValue value)
    {
      SensorValue result = value;
      switch (Type)
      {
        case SensorType.Compass:
          {
            result.Value = result.Value / 360.0f; // the normal range of a compass
            result.Value *= ScaleMax - ScaleMin;
            result.Value += ScaleMin;
            return result;
          }
      }
      return result;
    }

  }
}
