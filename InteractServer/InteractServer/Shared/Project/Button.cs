using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Shared.Project
{
    public class Button : JsonObject
    {
        public Color Background { get; set; } = new Color();
        public Color Foreground { get; set; } = new Color();

        public Button()
        {
#if INTERACTSERVER
            Background.Set(System.Windows.Media.Colors.White);
            Foreground.Set(System.Windows.Media.Colors.Black);
#endif
        }
        
    }
}
