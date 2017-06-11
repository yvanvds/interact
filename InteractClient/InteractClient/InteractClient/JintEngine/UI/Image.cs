using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{
    public class Image : Xamarin.Forms.Image
    {
        public Image()
        {

        }

        public void Set(String ImageName)
        {
            Source = Data.Project.Current.GetImage(ImageName).ImageSource;
        }
    }
}
