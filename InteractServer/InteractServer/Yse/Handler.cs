using IYse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Yse
{
	public class YseHandler : YapView.IYapHandler
	{
		public IPatcher patcher = null;
		private bool needsSaving = false;
		private OscTree.Object oscObject = null;

		public bool NeedsSaving { get => needsSaving; set => needsSaving = value; }

		public YseHandler(IPatcher patcher)
		{
			this.patcher = patcher;
			patcher.OnOscBang += Patcher_OnOscBang;
			patcher.OnOscFloat += Patcher_OnOscFloat;
			patcher.OnOscInt += Patcher_OnOscInt;
			patcher.OnOscString += Patcher_OnOscString;
		}

		private void Patcher_OnOscString(string to, string value)
		{
			Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.ID), new object[] { value });
		}

		private void Patcher_OnOscInt(string to, int value)
		{
			Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.ID), new object[] { value });
		}

		private void Patcher_OnOscFloat(string to, float value)
		{
			Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.ID), new object[] { value });
		}

		private void Patcher_OnOscBang(string to)
		{
			Osc.Tree.Root.Deliver(new OscTree.Route(to, OscTree.Route.RouteType.ID), new object[] { });
		}

		public void Load(string jsonContent)
		{
			patcher.ParseJSON(jsonContent);
			needsSaving = false;
		}

		public string Save()
		{
			needsSaving = false;
			return patcher.DumpJSON();
		}

		public void Clear()
		{
			patcher.Clear();
			needsSaving = true;
		}

		public void SetOscObject(OscTree.Object oscObject)
		{
			this.oscObject = oscObject;

			for(uint i = 0; i < NumObjects(); i++)
			{
				object obj = GetObjectFromList(i);
				string name = GetObjectName(obj);
				if(name.Equals(".r"))
				{
					AddOscEndpoint(obj);
				}
			}
		}

		private void AddOscEndpoint(object obj)
		{
			string args = GetObjectArguments(obj);
			oscObject.Endpoints.Add(new OscTree.Endpoint(args, (values) =>
			{
				if (values == null)
				{
					SendBang(obj);
				}
				else if (values[0] is float || values[0] is double)
				{
					SendFloatData(obj, (float)values[0]);
				}
				else if (values[0] is int)
				{
					SendIntData(obj, (int)values[0]);
				}
				else if (values[0] is string)
				{
					SendStringData(obj, (string)values[0]);
				}
			}
			));
		}

		public void Connect(object start, uint outlet, object end, uint inlet)
		{
			patcher.Connect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
			needsSaving = true;
		}

		public object CreateObject(string name)
		{
			needsSaving = true;
			return patcher.CreateObject(name);
		}

		public void DeleteObject(object obj)
		{
			needsSaving = true;
			string objName = GetObjectName(obj);
			if (objName.Equals(".r"))
			{
				string arg = GetObjectArguments(obj);
				if(oscObject.Endpoints.List.ContainsKey(arg))
				{
					oscObject.Endpoints.List.Remove(arg);
				}
			}
			patcher.DeleteObject((IHandle)obj);
		}

		public void Disconnnect(object start, uint outlet, object end, uint inlet)
		{
			needsSaving = true;
			try
			{
				patcher.Disconnect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
			}
			catch (Exception) { }
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
			string objName = GetObjectName(name);
			if(objName.Equals(".r"))
			{
				string objArgs = GetObjectArguments(name);
				if(oscObject.Endpoints.List.ContainsKey(objArgs))
				{
					oscObject.Endpoints.List.Remove(objArgs);
				}
			}
			((IHandle)name).SetArgs(args);
			if(objName.Equals(".r"))
			{
				AddOscEndpoint(name);
			}
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
