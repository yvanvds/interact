using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public class DiskResourceSerializationBinder : ISerializationBinder
  {
    public Type BindToType(string assemblyName, string typeName)
    {
      var resolvedTypeName = string.Format("{0}, {1}", typeName, assemblyName);
      return Type.GetType(resolvedTypeName, true);
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
      assemblyName = null;
      typeName = serializedType.AssemblyQualifiedName;
    }
  }

  public class ProjectFile
  {
    public int interactVersion;

    public Shared.Project.ConfigValues configValues = new Shared.Project.ConfigValues();
    public Shared.Project.Button configButton = new Shared.Project.Button();
    public Shared.Project.Background configPage = new Shared.Project.Background();
    public Shared.Project.Text configText = new Shared.Project.Text();
    public Shared.Project.Title configTitle = new Shared.Project.Title();

    public Collection<IDiskResource> DiskResources = new Collection<IDiskResource>();
  }
}
