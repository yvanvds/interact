using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Shared.Project
{
    public class Background : JsonObject
    {
        public Color Color { get; set; } = new Color();

        public Background()
        {
#if INTERACTSERVER
            Color.Set(System.Windows.Media.Colors.Black);
#endif
        }
    }
}
