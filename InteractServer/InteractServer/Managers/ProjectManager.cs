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

namespace InteractServer.Managers
{
  public class ProjectManager
  {
    public string LastFolder; // the last folder accessed by file dialogs. New dialogs will start here.
    public Project Current;


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
      string projectPath = FolderName + @"\" + DiskName + ".intp";

      // create database for project
      LoadProject(projectPath, ProjectName);

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
        LoadProject(dlg.FileName);
        LastFolder = Path.GetDirectoryName(dlg.FileName);
      }
    }

    private void LoadProject(string FileName, string CreateWithName = "")
    {
      SaveCurrentProject();
      Global.IntelliServerScripts.ClearAll();

      Current = new Project(FileName, CreateWithName);
      Current.InitIntellisense();
      Global.ProjectExplorerPage.Refresh();
    }

    private void SaveCurrentProject()
    {
      Current?.Save();
    }
  }
}
