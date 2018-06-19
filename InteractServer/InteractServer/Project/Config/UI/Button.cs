using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.Project.Config.UI
{
  public class Button
  {
    public Button(Shared.Project.Button backend)
    {
      this.backend = backend;
    }

    [Category("Brush")]
    [DisplayName("Background")]
    public Color Background
    {
      get => backend.Background.Get();
      set
      {
        backend.Background.Set(value);
        backend.Tainted = true;
        updateLink();
      }
    }

    [Category("Brush")]
    [DisplayName("Foreground")]
    public Color Foreground
    {
      get => backend.Foreground.Get();
      set
      {
        backend.Foreground.Set(value);
        backend.Tainted = true;
        updateLink();
      }
    }

    [Browsable(false)]
    public System.Windows.Controls.Button Link
    {
      get => link;
      set
      {
        link = value;
        updateLink();
      }
    }
    private System.Windows.Controls.Button link;

    [Browsable(false)]
    private Shared.Project.Button backend;

    private void updateLink()
    {
      link.Background = new SolidColorBrush(backend.Background.Get());
      link.Foreground = new SolidColorBrush(backend.Foreground.Get());
    }

    public String Serialize()
    {
      return backend.Serialize();
    }

    public void DeSerialize(String data)
    {
      backend.DeSerialize(data);
    }

    [Browsable(false)]
    public bool Tainted
    {
      get
      {
        return backend.Tainted;
      }
      set
      {
        backend.Tainted = value;
      }
    }

  }
}
