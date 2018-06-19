using InteractServer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InteractServer.Managers.AudioManager;

namespace InteractServer.Implementation.Logic
{
  class Patcher : Interact.Logic.Patcher
  {
    private Project.Patcher.Item patcher = null;
    private AudioObject audioObject = null;

    public override void Load(string path)
    {
      patcher = Global.ProjectManager.Current.Patchers.Get(path);

      if (patcher != null)
      {
        Global.AudioManager.AddObject(patcher);
        audioObject = Global.AudioManager.GetObject(patcher);
      }
      else
      {
        Global.Log.AddEntry("This project has no patcher named " + path);
      }
    }

    public override void EnableAudio()
    {
      if(audioObject != null) audioObject.EnableAudio = true;
    }

    public override void DisableAudio()
    {
      if (audioObject != null) audioObject.EnableAudio = false;
    }

    public override void PassBang(string to)
    {
      if (audioObject != null) audioObject.patchAudio.PassBang(to);
    }

    public override void PassInt(int value, string to)
    {
      if (audioObject != null) audioObject.patchAudio.PassData(value, to);
    }

    public override void PassFloat(float value, string to)
    {
      if (audioObject != null) audioObject.patchAudio.PassData(value, to);
    }

    public override void PassString(string value, string to)
    {
      if (audioObject != null) audioObject.patchAudio.PassData(value, to);
    }

    ~Patcher()
    {
      Dispose();
    }

    public override void Dispose()
    {
      if (patcher != null) Global.ViewManager.Close(patcher);
    }
  }
}
