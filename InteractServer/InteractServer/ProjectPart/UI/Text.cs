using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.ProjectPart.UI
{
    public class Text
    {
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

        [Category("Font")]
        [DisplayName("Size")]
        public int FontSize
        {
            get => backend.FontSize;
            set
            {
                backend.FontSize = value;
                backend.Tainted = true;
                updateLink();
            }
        }

        [Browsable(false)]
        public System.Windows.Controls.TextBlock Link
        {
            get => link;
            set
            {
                link = value;
                updateLink();
            }
        }
        private System.Windows.Controls.TextBlock link;

        [Browsable(false)]
        private Shared.Project.Text backend = new Shared.Project.Text();

        private void updateLink()
        {
            link.Background = new SolidColorBrush(backend.Background.Get());
            link.Foreground = new SolidColorBrush(backend.Foreground.Get());
            link.FontSize = backend.FontSize;
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
