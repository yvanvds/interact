using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.Osc.Values
{
  public static class OscValue
  {
    public static IOscValue Wrap(object obj)
    {
      if (obj == null)
      {
        return new OscNull();
      }
      else if (obj.GetType() == typeof(byte[]))
      {
        return new OscBlob(obj as byte[]);
      }
      else if (obj.GetType() == typeof(string))
      {
        return new OscString(obj as string);
      }
      else if (obj.GetType() == typeof(int))
      {
        return new OscInt((int)obj);
      }
      else if (obj.GetType() == typeof(float))
      {
        return new OscFloat((float)obj);
      }
      else if (obj.GetType() == typeof(double))
      {
        return new OscFloat(Convert.ToSingle(obj));
      }
      else if (obj.GetType() == typeof(DateTime))
      {
        return new OscTimeTag((DateTime)obj);
      }
      else if (obj.GetType() == typeof(Color))
      {
        return new OscColor(obj as Color);
      }
      else if (obj.GetType() == typeof(MidiMessage))
      {
        return new OscMidi(obj as MidiMessage);
      }
      else if (obj.GetType() == typeof(bool))
      {
        bool value = (bool)obj;
        if (value)
        {
          return new OscTrue();
        }
        else
        {
          return new OscFalse();
        }
      }
      else
      {
        Network.Sender.Get().WriteLog("OSC Error: " + obj.GetType() + " is not a legal OSC Value type");
        return new OscNull();
      }
    }

    public static IOscValue Parse(char typeTag, BinaryReader reader)
    {
      switch (typeTag)
      {
        case 'i':
          return OscInt.Parse(reader);
        case 'f':
          return OscFloat.Parse(reader);
        case 's':
          return OscString.Parse(reader);
        case 'b':
          return OscBlob.Parse(reader);
        case 'T':
          return new OscTrue();
        case 'F':
          return new OscFalse();
        case 'N':
          return new OscNull();
        case 'I':
          return new OscImpulse();
        case 't':
          return OscTimeTag.Parse(reader);
        case 'c':
          return OscColor.Parse(reader);
        case 'm':
          return OscMidi.Parse(reader);
        default:
          throw new ArgumentException("No such type tag as " + typeTag);
      }
    }
  }

}
