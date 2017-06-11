using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.UI
{

    public class Eventhandler
    {
        private Dictionary<Guid, String> Objects = new Dictionary<Guid, string>();

        public void Register(Guid ID, String name)
        {
            Objects.Add(ID, name);
        }

        public void Clear()
        {
            Objects.Clear();
        }


        public void OnClick(object sender, EventArgs e)
        {
            Xamarin.Forms.Element element = sender as Xamarin.Forms.Element;
            Engine.Instance.Invoke(Objects[element.Id]);
        }
    }
}
