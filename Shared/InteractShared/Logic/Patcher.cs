using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Logic
{
  public abstract class Patcher
  {
    public abstract void Load(string name);
    public abstract void EnableAudio();
    public abstract void DisableAudio();
    public abstract void Dispose();

    public abstract void PassBang(string to);
    public abstract void PassInt(int value, string to);
    public abstract void PassFloat(float value, string to);
    public abstract void PassString(string value, string to);
  }
}
