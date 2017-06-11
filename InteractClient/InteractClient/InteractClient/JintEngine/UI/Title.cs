using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{
    public class Title : Xamarin.Forms.Label
    {
        public Title()
        {
            BackgroundColor = Data.Project.Current.ConfigTitle.Background.Get();
            TextColor = Data.Project.Current.ConfigTitle.Foreground.Get();
            FontSize = Data.Project.Current.ConfigTitle.FontSize;
        }
    }
}
