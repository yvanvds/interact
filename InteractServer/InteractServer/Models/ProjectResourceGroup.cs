using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
    public class ProjectResourceGroup
    {
        public string Name { get; set; }
        public ObservableCollection<ProjectResource> Resources { get; set; }

        public string Icon { get; set; }

        public bool IsExpanded { get; set; }

        public string Count
        {
            get
            {
                return " [" + Resources.Count.ToString() + "]";
            }
        }

        public ProjectResourceGroup()
        {
            Resources = new ObservableCollection<ProjectResource>();
        }
    }
}
