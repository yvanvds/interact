

namespace InteractClient.UWP.Osc
{
  internal class OscLiteralEvent
  {
    /// <summary>
    /// The literal address of the event
    /// </summary>
    public readonly string Address;

    public event OscMessageEvent Event;

    public bool IsNull { get { return Event == null; } }

    internal OscLiteralEvent(string address)
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
