using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interact.UI;
using System.Windows.Media.Imaging;

namespace InteractServer.Implementation.UI
{
  public class Image : Interact.UI.Image
  {
    

    private System.Windows.Controls.Image UIObject = new System.Windows.Controls.Image();
    private Color backgroundColor = new Color();

    public override object InternalObject => UIObject;

    static Utils.ImageToBitmapSourceConverter converter = null;

    public Image()
    {
      UIObject.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
      UIObject.VerticalAlignment = System.Windows.VerticalAlignment.Center;

      if (converter == null) converter = new Utils.ImageToBitmapSourceConverter();
    }

    public override Interact.UI.Color BackgroundColor
    {
      get => backgroundColor;
      set
      {
        backgroundColor = new Color(value.R, value.G, value.B, value.A);
      }
    }

    public override void Set(string ImageName, Mode mode)
    {
      UIObject.Source = converter.Convert(Global.ProjectManager.Current.Images.Get(ImageName).ImageObj, null, null, null) as BitmapSource;
      switch (mode)
      {
        case Mode.Fill: UIObject.Stretch = System.Windows.Media.Stretch.UniformToFill; break;
        case Mode.Fit: UIObject.Stretch = System.Windows.Media.Stretch.Uniform; break;
        case Mode.Stretch: UIObject.Stretch = System.Windows.Media.Stretch.Fill; break;
      }
    }

    public override bool Visible
    {
      get => UIObject.Visibility == System.Windows.Visibility.Visible;
      set
      {
        if (value == true)
        {
          UIObject.Visibility = System.Windows.Visibility.Visible;
        }
        else
        {
          UIObject.Visibility = System.Windows.Visibility.Hidden;
        }
      }
    }
  }

}