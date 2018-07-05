using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Shared.Project
{
    public class Text : JsonObject
    {
        public Color Foreground { get; set; } = new Color();
        public Color Background { get; set; } = new Color();
        public int FontSize { get; set; }

        public Text()
        {
#if INTERACTSERVER
            Foreground.Set(System.Windows.Media.Colors.White);
            Background.Set(System.Windows.Media.Colors.Transparent);
            FontSize = 12;
#endif
        }
    }
}
