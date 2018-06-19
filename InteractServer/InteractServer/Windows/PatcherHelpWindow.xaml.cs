using InteractServer.Utils;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InteractServer.Windows
{
  /// <summary>
  /// Interaction logic for PatcherHelpWindow.xaml
  /// </summary>
  public partial class PatcherHelpWindow : MetroWindow
  {
    IYse.ISound sound;
    IYse.IPatcher patcher;

    String currentFileName = "";
    private object dummyNode = null;

    public PatcherHelpWindow()
    {
      InitializeComponent();

      sound = Global.Yse.NewSound();
      patcher = Global.Yse.NewPatcher();
      patcher.Create(1);

      yap.Handle = new YseHandler(patcher);
      yap.Focusable = true;
      yap.Focus();
      yap.Init();

      sound.Create(patcher);
      sound.Play();

      Title = "Patcher Help";

      string path = "";

#if DEBUG
      path = System.IO.Directory.GetCurrentDirectory();
      DirectoryInfo parent = System.IO.Directory.GetParent(path);
      parent = parent.Parent.Parent.Parent;
      path = parent.FullName;
      path = System.IO.Path.Combine(path, "patcherhelp");
#else
      path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      path = System.IO.Path.Combine(path, "patcherhelp");
#endif


      if (Directory.Exists(path))
      {
        foreach (string s in Directory.GetDirectories(path))
        {
          TreeViewItem item = new TreeViewItem();
          item.Header = s.Substring(s.LastIndexOf("\\") + 1);
          item.Tag = s;
          item.FontWeight = FontWeights.Normal;
          item.Items.Add(dummyNode);
          item.Expanded += new RoutedEventHandler(folderExpanded);
          helpfiles.Items.Add(item);
        }
      }
    }

    void folderExpanded(object sender, RoutedEventArgs e)
    {
      TreeViewItem item = (TreeViewItem)sender;
      if (item.Items.Count == 1 && item.Items[0] == dummyNode)
      {
        item.Items.Clear();
        try
        {
          foreach (string s in Directory.GetFiles(item.Tag.ToString()))
          {
            TreeViewItem subitem = new TreeViewItem();
            string header = s.Substring(s.LastIndexOf("\\") + 1);
            header = header.Remove(header.LastIndexOf('.'));
            header = header.Replace('_', ' ');
            subitem.Header = header;
            subitem.Tag = s;
            subitem.FontWeight = FontWeights.Normal;
            subitem.MouseDoubleClick += new MouseButtonEventHandler(openFile);
            item.Items.Add(subitem);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }

    void openFile(object sender, MouseButtonEventArgs e)
    {
      TreeViewItem item = (TreeViewItem)sender;

      string content = File.ReadAllText(item.Tag.ToString());
      LoadContent(item.Tag.ToString(), content);
    }

    protected override void OnClosed(EventArgs e)
    {
      yap.Clear();
      base.OnClosed(e);

      patcher.Dispose();
      sound.Dispose();
    }

    public void LoadContent(string filename, string content)
    {
      yap.Clear();
      patcher.Clear();
      patcher.ParseJSON(content);
      yap.Load();
      currentFileName = filename;
      YapView.Interface.PerformanceMode = true;
    }
  }
}
