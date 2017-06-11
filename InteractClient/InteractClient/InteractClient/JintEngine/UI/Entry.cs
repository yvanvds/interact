using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{
  class Entry : Xamarin.Forms.Entry
  {
    public Entry()
    {
      BackgroundColor = Data.Project.Current.ConfigText.Background.Get();
      TextColor = Data.Project.Current.ConfigText.Foreground.Get();
      FontSize = Data.Project.Current.ConfigText.FontSize;
    }
  }
}
