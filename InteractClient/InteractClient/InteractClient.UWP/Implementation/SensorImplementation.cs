using InteractClient.UWP.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InteractClient.Interface;
using Interact;

[assembly: Dependency(typeof(SensorImplementation))]
namespace InteractClient.UWP.Implementation
{
  public class SensorImplementation : ISensor
  {
    public event SensorValueChangedEventHandler SensorValueChanged;

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public bool IsActive(SensorType sensorType)
    {
      throw new NotImplementedException();
    }

    public void Start(SensorType sensorType, SensorDelay interval = SensorDelay.Default)
    {
      throw new NotImplementedException();
    }

    public void Stop(SensorType sensorType)
    {
      throw new NotImplementedException();
    }
  }
}
