using System.Windows;

namespace InteractServer.Dialogs
{
	/// <summary>
	/// Interaction logic for RouteSelector.xaml
	/// </summary>
	public partial class RouteSelector : MahApps.Metro.Controls.MetroWindow
	{
		public OscTree.Route CurrentRoute = null;
		private OscTree.Object Origin = null;
		private OscTree.Tree root;
		private bool storeValueOverride;
		private string valueOverrideMethod;
		private bool serverSide = true;

		public RouteSelector(OscTree.Object origin, OscTree.Tree root)
		{
			InitializeComponent();
			Origin = origin;
			SidePanelVisible(false, false);

			SetRoot(root);

			serverSide = Osc.Tree.Server.Contains(origin);
		}

		private void SetRoot(OscTree.Tree root)
		{
			this.root = root;
			TreeGui.SetRoot(root);
			TreeGui.OnRouteChanged += () =>
			{
				EvaluateRoute();
			};
		}

		private void EvaluateRoute()
		{
			OkButton.IsEnabled = (TreeGui.SelectedRoute != null);
			if (TreeGui.SelectedRoute != null)
			{
				if (TreeGui.SelectedRoute.Steps.Count > 1 && TreeGui.SelectedRoute.Steps[1].Equals("Client"))
				{
					SidePanelVisible(true, true);

					OscTree.Route namedRoute = CreateNamedRoute(TreeGui.SelectedRoute);
					CurrentRouteName.Content = namedRoute.GetActualRoute();
					TreeGui.SelectedRoute.ScreenName = CurrentRouteName.Content as string;
				}
				else
				{
					SidePanelVisible(false, true);
					CurrentRouteName.Content = root.GetNameOfRoute(TreeGui.SelectedRoute);
					TreeGui.SelectedRoute.ScreenName = CurrentRouteName.Content as string;
				}
			}
			else
			{
				CurrentRouteName.Content = "";
				SidePanelVisible(false, false);
			}
		}

		public void SidePanelVisible(bool overrideClient, bool overrideValue)
		{
			if(overrideClient == false && overrideValue == false)
			{
				SidePanel.Visibility = Visibility.Hidden;
			} else
			{
				SidePanel.Visibility = Visibility.Visible;
			}

			if(overrideClient)
			{
				OverrideClient.Visibility = Visibility.Visible;
				if (TreeGui.SelectedRoute.Replacements == null)
				{
					TreeGui.SelectedRoute.Replacements = new System.Collections.Generic.Dictionary<int, string>();
					TreeGui.SelectedRoute.Replacements[1] = "LocalClient";
				}
				else
				{
					if (TreeGui.SelectedRoute.Replacements.ContainsKey(1))
					{
						switch (TreeGui.SelectedRoute.Replacements[1])
						{
							case "AllClients":
								RBAll.IsChecked = true;
								break;
							case "LocalClient":
								RBLocal.IsChecked = true;
								break;
							default:
								RBGroup.IsChecked = true;
								break;
						}
					}
				}
			} else
			{
				OverrideClient.Visibility = Visibility.Collapsed;
			}

			if(overrideValue)
			{
				ScriptView.Visibility = Visibility.Visible;
				if(TreeGui.SelectedRoute.ValueOverrideMethodName != string.Empty)
				{
					CurrentPathOverride.Visibility = Visibility.Visible;
					CurrentPathOverride.Content = "Current: " + TreeGui.SelectedRoute.ValueOverrideMethodName;
					CreatePathOverride.Visibility = Visibility.Collapsed;
					ChangePathOverride.Visibility = Visibility.Visible;
					DeletePathOverride.Visibility = Visibility.Visible;
				} else
				{
					CurrentPathOverride.Visibility = Visibility.Collapsed;
					CreatePathOverride.Visibility = Visibility.Visible;
					ChangePathOverride.Visibility = Visibility.Collapsed;
					DeletePathOverride.Visibility = Visibility.Collapsed;
				}
			} else
			{
				ScriptView.Visibility = Visibility.Collapsed;
			}

			if(overrideValue && overrideClient)
			{
				Line.Visibility = Visibility.Visible;
			} else
			{
				Line.Visibility = Visibility.Collapsed;
			}
		}

		public void SetRoute(OscTree.Route route)
		{
			CurrentRoute = route;
			TreeGui.SetRoute(route);
			OscTree.Route namedRoute = CreateNamedRoute(TreeGui.SelectedRoute);
			CurrentRouteName.Content = namedRoute.GetActualRoute();
		}

		private void CancelClicked(object sender, RoutedEventArgs e)
		{
			if(serverSide)
			{
				Project.Project.Current.ServerEndpointWriter.DiscardChanges();
			} else
			{
				Project.Project.Current.ClientEndpointWriter.DiscardChanges();
			}
			DialogResult = false;
			Close();
		}

		private void OkClicked(object sender, RoutedEventArgs e)
		{
			CurrentRoute = TreeGui.SelectedRoute;
			if(storeValueOverride)
			{
				CurrentRoute.ValueOverrideMethodName = valueOverrideMethod;

				if(serverSide)
				{
					Project.Project.Current.ServerEndpointWriter.Save();
					Project.Project.Current.RecompileServerScripts();
				} else
				{
					Project.Project.Current.ClientEndpointWriter.Save();
					Project.Project.Current.RecompileClientScripts();
				}
				
			}
			DialogResult = true;
			Close();
		}

		private void OverrideButton_Click(object sender, RoutedEventArgs e)
		{
			if (TreeGui.SelectedRoute.Replacements == null)
			{
				TreeGui.SelectedRoute.Replacements = new System.Collections.Generic.Dictionary<int, string>();
			}

			if (RBAll.IsChecked == true)
			{
				TreeGui.SelectedRoute.Replacements[1] = "AllClients";
			} else if (RBLocal.IsChecked == true)
			{
				TreeGui.SelectedRoute.Replacements[1] = "LocalClient";
			} else if (RBGroup.IsChecked == true)
			{
				TreeGui.SelectedRoute.Replacements[1] = "Group";
			}

			OscTree.Route namedRoute = CreateNamedRoute(TreeGui.SelectedRoute);
			CurrentRouteName.Content = namedRoute.GetActualRoute();
			TreeGui.SelectedRoute.ScreenName = CurrentRouteName.Content as string;
		}

		private OscTree.Route CreateNamedRoute(OscTree.Route source)
		{
			source.CurrentStep = 0;
			OscTree.Route route = new OscTree.Route(root.GetNameOfRoute(source), OscTree.Route.RouteType.NAME);
			route.Replacements = source.Replacements;
			return route;
		}

		private void CreateValueOverride_Click(object sender, RoutedEventArgs e)
		{
			string arg = Origin.DataType.ToString();
			if (arg == "System.Single") arg = "float";
			arg += " value";

			TreeGui.SelectedRoute.CurrentStep = 0;
			var target = Osc.Tree.Root.GetEndpoint(TreeGui.SelectedRoute);
			if(target == null)
			{
				Dialogs.Error.Show("Routing Error", "No Valid Target");
				return;
			}

			if(target.ValidArgs.Count == 0)
			{
				Dialogs.Error.Show("Routing Error", "Target has no valid type.");
				return;
			}

			string result = target.ValidArgs[0].ToString();
			if (result == "System.Single") result = "float";

			var dialog = new Dialogs.GetString("New Method Name");
			dialog.ShowDialog();
			if(dialog.Result == string.Empty)
			{
				return;
			}
			valueOverrideMethod = dialog.Result;

			string method = "public static "  + result + " " + valueOverrideMethod + "(" + arg + ")";

			CodeEditor.CodeEditor view;
			if (serverSide)
			{
				view = Project.Project.Current.ServerEndpointWriter.View;
			} else
			{
				view = Project.Project.Current.ClientEndpointWriter.View;
			}

			view.InsertMethod(method);
			view.SetFocusOnNewCode();

			var window = new DocumentAsDialog("EndpointCode", view);
			
			window.ShowDialog();
			if(window.DialogResult == true)
			{
				storeValueOverride = true;
				
			} else
			{
				if(serverSide)
				{
					Project.Project.Current.ServerEndpointWriter.DiscardChanges();
				} else
				{
					Project.Project.Current.ClientEndpointWriter.DiscardChanges();
				}
				
			}
		}

		private void DeletePathOverride_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ChangePathOverride_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
