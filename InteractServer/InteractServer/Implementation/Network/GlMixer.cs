using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Implementation.Network
{
  public class GlMixer : Interact.Network.GlMixer
  {
    private OscSender Sender = new OscSender();

    private float alpha;
    public override float RenderAlpha
    {
      get
      {
        return alpha;
      }
      set
      {
        alpha = value;
        Sender.Send("/glmixer/render/Alpha", alpha);
      }
    }

    private bool pause;
    public override bool Paused
    {
      get
      {
        return pause;
      }

      set
      {
        pause = value;
        Sender.Send("/glmixer/render/Pause", pause);
      }
    }

    public override void Connect(string IPAddress, int port)
    {
      Sender.Init(IPAddress, port);
    }

    public override void Alpha(string source, float alpha)
    {
      Sender.Send("/glmixer/" + source + "/Alpha", alpha);
    }

    public override void AlphaCoordinates(string source, float X, float Y)
    {
      Sender.Send("/glmixer/" + source + "/AlphaCoordinates", X, Y);
    }

    public override void Geometry(string source, float PosX, float PosY, float ScaleX, float ScaleY, float Angle)
    {
      Sender.Send("/glmixer/" + source + "/Geometry", PosX, PosY, ScaleX, ScaleY, 0.0, 0.0, Angle); 
    }

    public override void Saturation(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Saturation", value);
    }

    public override void Brightness(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Brightness", value);
    }

    public override void Contrast(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Contrast", value);
    }

    public override void HueShift(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/HueShift", value);
    }

    public override void TreshHold(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/TreshHold", value);
    }

    public override void Posterized(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Posterized", value);
    }

    public override void Pixelated(string source, bool value)
    {
      Sender.Send("/glmixer/" + source + "/Pixelated", value);
    }

    public override void Mask(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Mask", value);
    }

    public override void Filter(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/Filter", value);
    }

    public override void Color(string source, int R, int G, int B)
    {
      Sender.Send("/glmixer/" + source + "/Color", R, G, B);
    }

    public override void ChromaKey(string source, bool value)
    {
      Sender.Send("/glmixer/" + source + "/ChromaKey", value);
    }

    public override void ChromaKeyTolerance(string source, int value)
    {
      Sender.Send("/glmixer/" + source + "/ChromaKeyTolerance", value);
    }

    public override void ChromaKeyColor(string source, int R, int G, int B)
    {
      Sender.Send("/glmixer/" + source + "/ChromaKeyColor", R, G, B);
    }

    public override void Gamma(string source, float Gamma, float MinInput, float MaxInput, float MinOutput, float MaxOutput)
    {
      Sender.Send("/glmixer/" + source + "Gamma", Gamma, MinInput, MaxInput, MinOutput, MaxOutput);
    }
  }
}
