using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Project
{
    public class Title : JsonObject
    {
        public Color Foreground { get; set; } = new Color();
        public Color Background { get; set; } = new Color();
        public int FontSize { get; set; }

        public Title()
        {
#if INTERACTSERVER
            Foreground.Set(System.Windows.Media.Colors.Black);
            Background.Set(System.Windows.Media.Colors.Transparent);
            FontSize = 16;
#endif
        }
    }
}
