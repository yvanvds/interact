using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Project
{
	public class PatcherModule : BaseModule
	{
		IYse.ISound sound;
		IYse.IPatcher patcher;
		private bool disposed = false;
		OscTree.Object osc;

		public PatcherModule()
		{
			sound = Global.Yse.NewSound();
			patcher = Global.Yse.NewPatcher();
			patcher.Create(1);
			sound.Create(patcher);
			
		}

		public override void LoadContent()
		{
			if(osc == null)
			{
				osc = new OscTree.Object(new OscTree.Address(Name, ID));
				Global.OscLocal.Add(osc);
			}

			sound.Stop();
			patcher.Clear();
			patcher.ParseJSON(Content);

			osc.Endpoints.List.Clear();
			for(uint i = 0; i < patcher.NumObjects(); i++)
			{
				object obj = patcher.GetHandleFromList(i);
				string name = ((IYse.IHandle)obj).Name;
				if(name.Equals(".r"))
				{
					string args = ((IYse.IHandle)obj).GetArgs();
					osc.Endpoints.Add(new OscTree.Endpoint(args, (values) =>
					{
						if(values == null)
						{
							((IYse.IHandle)obj).SetBang(0);
						}
						else if(values[0] is int)
						{
							((IYse.IHandle)obj).SetIntData(0, (int)values[0]);
						}
						else if (values[0] is float)
						{
							((IYse.IHandle)obj).SetFloatData(0, (float)values[0]);
						}
						else if (values[0] is string)
						{
							((IYse.IHandle)obj).SetListData(0, (string)values[0]);
						}
					}));
				}
			}
		}

		public void EnableAudio()
		{
			sound.Volume = 1.0f;
			sound.Play();
		}

		public void DisableAudio()
		{
			sound.FadeAndStop(100);
		}

		public void Clear()
		{
			sound.Stop();
			patcher.Clear();
		}

		public void Dispose()
		{
			sound.Stop();
			patcher.Dispose();
			sound.Dispose();
			disposed = true;
		}

		public override void Activate()
		{
			
		}

		~PatcherModule()
		{
			if(!disposed) Dispose();
		}
	}
}
