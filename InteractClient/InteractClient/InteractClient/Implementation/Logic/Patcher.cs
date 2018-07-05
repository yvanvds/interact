using System;
using System.Collections.Generic;
using System.Text;

namespace InteractClient.Implementation.Logic
{
  public class Patcher : Interact.Logic.Patcher
  {
    private IYse.ISound sound;
    private IYse.IPatcher patcher;

    public Patcher()
    {
			sound = Global.Yse.NewSound();
			patcher = Global.Yse.NewPatcher();
			patcher.Create(1);
			sound.Create(patcher);
		}

    ~Patcher()
    {
      sound.Dispose();
      patcher.Dispose();
    }

    public override void DisableAudio()
    {
      sound.FadeAndStop(100);
			//Global.Yse.System.AudioTest = false;
		}

    public override void EnableAudio()
    {
			sound.Play();
			//Global.Yse.System.AudioTest = true;
		}

    public override void Load(string name)
    {
			Data.Patcher p = Data.Project.Current.GetPatcher(name);
      if(p != null)
      {
        patcher.ParseJSON(p.Content);
			}
		}

    public override void Dispose()
    {
      sound.Stop();
      patcher.Dispose();
      sound.Dispose();
    }

    public override void PassBang(string to)
    {
      patcher.PassBang(to);
    }

    public override void PassFloat(float value, string to)
    {
			patcher.PassData(value, to);
		}

    public override void PassInt(int value, string to)
    {
      patcher.PassData((int)value, to);
    }

    public override void PassString(string value, string to)
    {
      patcher.PassData(value, to);
    }
  }
}
