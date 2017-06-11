using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Project
{
    public class Color
    {
        public float A { get; set; }
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

#if INTERACTSERVER
        public void Set(System.Windows.Media.Color source)
        {
            A = source.ScA;
            R = source.ScB;
            G = source.ScG;
            B = source.ScB;
        }

        public System.Windows.Media.Color Get()
        {
            return System.Windows.Media.Color.FromScRgb(A, R, G, B);
        }
#elif INTERACTCLIENT
        public Xamarin.Forms.Color Get()
        {
            return Xamarin.Forms.Color.FromRgba(R, G, B, A);
        }
#endif
    }
}
