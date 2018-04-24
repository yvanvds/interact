using IYse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Utils
{
  public class YseHandler : YapView.IYapHandler
  {
    public IPatcher patcher = null;

    public YseHandler(IPatcher patcher)
    {
      this.patcher = patcher;
    }

    public void Load(string jsonContent)
    {
      patcher.ParseJSON(jsonContent);

    }

    public string Save()
    {
      return patcher.DumpJSON();
    }

    public void Clear()
    {
      patcher.Clear();
    }

    public void Connect(object start, uint outlet, object end, uint inlet)
    {
      patcher.Connect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
    }

    public object CreateObject(string name)
    {
      return patcher.CreateObject(name);
    }

    public void DeleteObject(object obj)
    {
      patcher.DeleteObject((IHandle)obj);
    }

    public void Disconnnect(object start, uint outlet, object end, uint inlet)
    {
      patcher.Disconnect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
    }

    public int GetObjectInputCount(object name)
    {
      return ((IHandle)name).Inputs;
    }
    public int GetObjectOutputCount(object name)
    {
      return ((IHandle)name).Outputs;
    }

    public void PassArgument(object name, string args)
    {
      ((IHandle)name).SetArgs(args);
    }

    public uint NumObjects()
    {
      return patcher.NumObjects();
    }

    public object GetObjectFromList(uint obj)
    {
      return patcher.GetHandleFromList(obj);
    }

    public string GetObjectName(object obj)
    {
      return ((IHandle)obj).Name;
    }

    public string GetObjectArguments(object obj)
    {
      return ((IHandle)obj).GetArgs();
    }

    public uint GetConnections(object obj, uint outlet)
    {
      return ((IHandle)obj).GetConnections(outlet);
    }

    public uint GetConnectionTarget(object obj, uint outlet, uint connection)
    {
      return ((IHandle)obj).GetConnectionTarget(outlet, connection);
    }

    public uint GetConnectionTargetInlet(object obj, uint outlet, uint connection)
    {
      return ((IHandle)obj).GetConnectionTargetInlet(outlet, connection);
    }

    public uint GetObjectID(object obj)
    {
      return ((IHandle)obj).GetID();
    }

    public void SendBang(object obj)
    {
      ((IHandle)obj).SetBang(0);
    }

    public void SendIntData(object obj, int value)
    {
      ((IHandle)obj).SetIntData(0, value);
    }

    public void SendFloatData(object obj, float value)
    {
      ((IHandle)obj).SetFloatData(0, value);
    }

    public void SendStringData(object obj, string value)
    {
      ((IHandle)obj).SetListData(0, value);
    }

    public string GetGuiValue(object obj)
    {
      if (obj == null) return "";
      return ((IHandle)obj).GetGuiValue();
    }

    public YapView.ObjectType GetObjectType(object obj)
    {
      switch (((IHandle)obj).Name)
      {
        case ".i": return YapView.ObjectType.INT;
        case ".f": return YapView.ObjectType.FLOAT;
        case ".slider": return YapView.ObjectType.SLIDER;
        case ".b": return YapView.ObjectType.BUTTON;
        case ".t": return YapView.ObjectType.TOGGLE;
        case ".counter": return YapView.ObjectType.COUNTER;
        case ".m": return YapView.ObjectType.MESSAGE;
        case ".text": return YapView.ObjectType.TEXT;
        default: return YapView.ObjectType.BASE;
      }
    }

    public string GetGuiProperty(object obj, string key)
    {
      return ((IHandle)obj).GetGuiProperty(key);
    }

    public void SetGuiProperty(object obj, string key, string value)
    {
      ((IHandle)obj).SetGuiProperty(key, value);
    }
  }
}
