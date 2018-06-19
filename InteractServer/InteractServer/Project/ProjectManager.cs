using InteractServer.Dialogs;
using InteractServer.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Project
{
  public class ProjectManager
  {
    public string LastFolder; // the last folder accessed by file dialogs. New dialogs will start here.
    public Project Current;

    const int Version = 1;

    public ProjectManager()
    {
      LastFolder = "";
    }

    public void StartNewProject()
    {
      NewProjectDialog dlg = new NewProjectDialog();
      dlg.ShowDialog();
    }

    public void CreateNewProject(String FolderName, String DiskName, String ProjectName)
    {
      string projectPath = Path.Combine(FolderName, DiskName);
      Directory.CreateDirectory(projectPath);

      string filePath = Path.Combine(projectPath, DiskName + ".intp");

      string content = Resources.templates.Template.Get("project.json");
      content = content.Replace("@name", ProjectName);
      content = content.Replace("@version", Version.ToString());
      content = content.Replace("@id", Guid.NewGuid().ToString());
      File.WriteAllText(filePath, content);

      LoadProject(filePath);
    }

    public void OpenProject()
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.CheckFileExists = true;
      dlg.Filter = "Interact project file (*.intp)|*.intp";
      if (LastFolder.Length > 0)
      {
        dlg.InitialDirectory = LastFolder;
      }

      if (dlg.ShowDialog() == true)
      {
        OpenProject(dlg.FileName);
      }
    }

    public void OpenProject(string FileName)
    {
      LoadProject(FileName);
      LastFolder = Path.GetDirectoryName(FileName);
    }

    private void LoadProject(string FileName)
    {
      SaveCurrentProject();
      Global.IntelliServerScripts.ClearAll();

      Current = new Project(FileName);
      if (Current.Valid)
      {
        Current.InitIntellisense();
        Global.ProjectExplorerPage.Refresh();
      }
      else Current = null;
    }

    public void SaveCurrentProject()
    {
      Current?.Save();
    }
  }
}
