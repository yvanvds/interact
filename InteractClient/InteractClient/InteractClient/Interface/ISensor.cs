using Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InteractClient.Interface
{
  public interface ISensor : IDisposable
  {
    event SensorValueChangedEventHandler SensorValueChanged;

    void Start(SensorType sensorType, SensorDelay interval = SensorDelay.Default);
    void Stop(SensorType sensorType);

    bool IsActive(SensorType sensorType);
  }

  public class SensorValueChangedEventArgs : EventArgs
  {
    public SensorType SensorType { get; set; }
    public Interact.Utility.SensorValue Value { get; set; }
    public SensorValueType ValueType { get; set; }
  }

  public delegate void SensorValueChangedEventHandler(object sender, SensorValueChangedEventArgs e);

  

}
