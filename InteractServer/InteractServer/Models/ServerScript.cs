using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
  public class ServerScript : ProjectResource
  {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string Content { get; set; }
  }
}
