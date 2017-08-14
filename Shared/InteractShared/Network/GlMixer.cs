using System;
using System.Collections.Generic;
using System.Text;

namespace Interact.Network
{
  public abstract class GlMixer
  {
    public abstract void Connect(string IPAddress, int port);

    public abstract float RenderAlpha { get; set; }
    public abstract bool  Paused { get; set; }

    public abstract void Alpha(string source, float alpha);
    public abstract void AlphaCoordinates(string source, float X, float Y);
    public abstract void Geometry(string source, float PosX, float PosY, float ScaleX, float ScaleY, float Angle);
    public abstract void Saturation(string source, int value); // -100 - 100
    public abstract void Brightness(string source, int value); // -100 - 100
    public abstract void Contrast(string source, int value); // -100 - 100
    public abstract void HueShift(string source, int value); // 0 - 360
    public abstract void TreshHold(string source, int value); // 0 - 100
    public abstract void Posterized(string source, int value); // 0 - 255
    public abstract void Pixelated(string source, bool value);
    public abstract void Mask(string source, int value); // 0 - 26
    public abstract void Filter(string source, int value); // 0 - 15
    public abstract void Color(string source, int R, int G, int B); // 0 - 255
    public abstract void ChromaKey(string source, bool value);
    public abstract void ChromaKeyTolerance(string source, int value); // 0 - 100
    public abstract void ChromaKeyColor(string source, int R, int G, int B); // 0 - 255
    public abstract void Gamma(string source, float Gamma, float MinInput, float MaxInput, float MinOutput, float MaxOutput); // 0 - 50, 0 - 1
  }
}
