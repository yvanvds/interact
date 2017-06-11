using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.ProjectPart.UI
{
    public class Page
    {
        [Category("Brush")]
        [DisplayName("Color")]
        public Color Color
        {
            get => backend.Color.Get();
            set
            {
                backend.Color.Set(value);
                backend.Tainted = true;
                updateLink();
            }
        }

        [Browsable(false)]
        public System.Windows.Controls.Grid Link
        {
            get => link;
            set
            {
                link = value;
                updateLink();
            }
        }
        private System.Windows.Controls.Grid link;

        [Browsable(false)]
        private Shared.Project.Background backend = new Shared.Project.Background();

        private void updateLink()
        {
            link.Background = new SolidColorBrush(backend.Color.Get());
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
