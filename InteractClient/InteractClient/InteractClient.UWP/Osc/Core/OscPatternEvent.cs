using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  internal class OscPatternEvent
  {
    /// <summary>
    /// The pattern address of the event
    /// </summary>
    public readonly OscAddress Address;

    public event OscMessageEvent Event;

    public bool IsNull { get { return Event == null; } }

    internal OscPatternEvent(OscAddress address)
    {
      Address = address;
      Event = null;
    }

    /// <summary>
    /// Invoke the event
    /// </summary>
    /// <param name="message">message that caused the event</param>
    public void Invoke(OscMessage message)
    {
      if (Event != null)
      {
        Event(message);
      }
    }

    /// <summary>
    /// Nullify the event
    /// </summary>
    public void Clear()
    {
      Event = null;
    }
  }
}
