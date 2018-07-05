using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Interface
{
  public class PressedEventArgs : EventArgs
  {
    public float Pressure { get; set; }
  }

  public class CCButton : Xamarin.Forms.Button
  {
    public new event EventHandler Pressed;
    public new event EventHandler Released;

    public virtual void OnPressed(float pressure)
    {
      
      Pressed?.Invoke(this, new PressedEventArgs()
      {
        Pressure = pressure,
      });
    }

    public virtual void OnReleased()
    {
      Released?.Invoke(this, EventArgs.Empty);
    }
  }
}
