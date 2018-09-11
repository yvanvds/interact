using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using ActiproSoftware.Text;
using ActiproSoftware.Text.Implementation;
using ActiproSoftware.Text.Languages.DotNet.Implementation;
using ActiproSoftware.Windows;
using ActiproSoftware.Windows.Controls.SyntaxEditor;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using ActiproSoftware.Windows.Themes;

namespace InteractServer.CodeEditor
{
	/// <summary>
	/// Provides some helper methods.
	/// </summary>
	public static class SyntaxEditorHelper
	{

		public const string DefinitionPath = "InteractServer.Resources.Definitions.";
		public const string SnippetsPath = "ActiproSoftware.ProductSamples.SyntaxEditorSamples.Languages.Snippets.";
		public const string ThemesPath = "InteractServer.Resources.Definitions.";
		public const string XmlSchemasPath = "ActiproSoftware.ProductSamples.SyntaxEditorSamples.Languages.XmlSchemas.";

		private static bool isDarkThemeActive;

		/// <summary>
		/// Creates an <see cref="ICodeSnippetFolder"/> and initializes it with specified code snippets from embedded resources.
		/// </summary>
		/// <param name="folderName">The folder name.</param>
		/// <param name="paths">The array of resource paths to load.</param>
		/// <returns>The <see cref="ICodeSnippetFolder"/> that was loaded.</returns>
		private static ICodeSnippetFolder LoadCodeSnippetFolderFromResources(string folderName, string[] paths)
		{
			ICodeSnippetFolder folder = new CodeSnippetFolder(folderName);
			CodeSnippetSerializer serializer = new CodeSnippetSerializer();

			foreach (string path in paths)
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
				{
					if (stream != null)
					{
						IEnumerable<ICodeSnippet> snippets = serializer.LoadFromStream(stream);
						if (snippets != null)
						{
							foreach (ICodeSnippet snippet in snippets)
								folder.Items.Add(snippet);
						}
					}
				}
			}

			return folder;
		}

		/// <summary>
		/// Initializes an existing <see cref="ISyntaxLanguage"/> from a language definition (.langdef file) from a resource stream.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public static void InitializeLanguageFromResourceStream(ISyntaxLanguage language, string filename)
		{
			string path = DefinitionPath + filename;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				if (stream != null)
				{
					SyntaxLanguageDefinitionSerializer serializer = new SyntaxLanguageDefinitionSerializer();
					serializer.InitializeFromStream(language, stream);
				}
			}
		}

		/// <summary>
		/// Loads a language definition (.langdef file) from a resource stream.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>The <see cref="ISyntaxLanguage"/> that was loaded.</returns>
		public static ISyntaxLanguage LoadLanguageDefinitionFromResourceStream(string filename)
		{
			string path = DefinitionPath + filename;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				if (stream != null)
				{
					SyntaxLanguageDefinitionSerializer serializer = new SyntaxLanguageDefinitionSerializer();
					return serializer.LoadFromStream(stream);
				}
				else
					return SyntaxLanguage.PlainText;
			}
		}

		/// <summary>
		/// Creates an <see cref="ICodeSnippetFolder"/> and initializes it with some sample code snippets from embedded resources.
		/// </summary>
		/// <returns>The <see cref="ICodeSnippetFolder"/> that was loaded.</returns>
		public static ICodeSnippetFolder LoadSampleCSharpCodeSnippetsFromResources()
		{
			// NOTE: If you have file system access, the static CodeSnippetFolder.LoadFrom(path) method easily
			//       loads snippets within a specified file path and should be used instead

			string[] childPaths = new string[] {
				SnippetsPath + "CSharp.Sample_Child_Folder.while.snippet",
			};
			ICodeSnippetFolder childFolder = LoadCodeSnippetFolderFromResources("Sample Child Folder", childPaths);

			string[] rootPaths = new string[] {
				SnippetsPath + "CSharp.for.snippet",
				SnippetsPath + "CSharp.switch.snippet",
			};
			ICodeSnippetFolder rootFolder = LoadCodeSnippetFolderFromResources("Root", rootPaths);
			rootFolder.Folders.Add(childFolder);
			return rootFolder;
		}

		/// <summary>
		/// Creates an <see cref="ICodeSnippetFolder"/> and initializes it with some sample code snippets from embedded resources.
		/// </summary>
		/// <returns>The <see cref="ICodeSnippetFolder"/> that was loaded.</returns>
		public static ICodeSnippetFolder LoadSampleJavascriptCodeSnippetsFromResources()
		{
			// NOTE: If you have file system access, the static CodeSnippetFolder.LoadFrom(path) method easily
			//       loads snippets within a specified file path and should be used instead

			string[] rootPaths = new string[] {
				SnippetsPath + "Javascript.JavascriptFor.snippet",
				SnippetsPath + "Javascript.JavascriptWhile.snippet",
			};
			ICodeSnippetFolder rootFolder = LoadCodeSnippetFolderFromResources("Root", rootPaths);
			return rootFolder;
		}

		/// <summary>
		/// Creates an <see cref="ICodeSnippetFolder"/> and initializes it with some sample code snippets from embedded resources.
		/// </summary>
		/// <returns>The <see cref="ICodeSnippetFolder"/> that was loaded.</returns>
		public static ICodeSnippetFolder LoadSampleVBCodeSnippetsFromResources()
		{
			// NOTE: If you have file system access, the static CodeSnippetFolder.LoadFrom(path) method easily
			//       loads snippets within a specified file path and should be used instead

			string[] childPaths = new string[] {
				SnippetsPath + "VB.Sample_Child_Folder.VBWhile.snippet",
			};
			ICodeSnippetFolder childFolder = LoadCodeSnippetFolderFromResources("Sample Child Folder", childPaths);

			string[] rootPaths = new string[] {
				SnippetsPath + "VB.VBFor.snippet",
				SnippetsPath + "VB.VBSelect.snippet",
			};
			ICodeSnippetFolder rootFolder = LoadCodeSnippetFolderFromResources("Root", rootPaths);
			rootFolder.Folders.Add(childFolder);
			return rootFolder;
		}

		/// <summary>
		/// Updates the image set for a theme change.
		/// </summary>
		public static void UpdateImageSetForThemeChange()
		{
			if (ThemeManager.CurrentTheme == ThemeName.MetroDark.ToString())
				CommonImageSourceProvider.DefaultImageSet = CommonImageSet.MetroDark;
			else if ((ThemeManager.CurrentTheme != null) && (ThemeManager.CurrentTheme.StartsWith("Metro")))
				CommonImageSourceProvider.DefaultImageSet = CommonImageSet.MetroLight;
			else
				CommonImageSourceProvider.DefaultImageSet = CommonImageSet.Classic;
		}

		/// <summary>
		/// Updates the highlighting style registry for a theme change.
		/// </summary>
		public static void UpdateHighlightingStyleRegistryForThemeChange()
		{
			var oldIsDarkThemeActive = isDarkThemeActive;
			isDarkThemeActive = (ThemeManager.CurrentTheme == ThemeName.MetroDark.ToString());

			if (isDarkThemeActive != oldIsDarkThemeActive)
			{
				// Unregister all classification types
				var classificationTypes = AmbientHighlightingStyleRegistry.Instance.ClassificationTypes.ToArray();
				foreach (var classificationType in classificationTypes)
					AmbientHighlightingStyleRegistry.Instance.Unregister(classificationType);

				// Re-register common classification types
				new DisplayItemClassificationTypeProvider().RegisterAll();
				new DotNetClassificationTypeProvider().RegisterAll();

				// Load HTML and XAML languages just so their custom classification types get re-registered
				//LoadLanguageDefinitionFromResourceStream("Html.langdef");
				//LoadLanguageDefinitionFromResourceStream("Xaml.langdef");
				// NOTE: Any other languages that are active would need to reload to ensure their custom classification types get re-registered as well

				if (isDarkThemeActive)
				{
					// Load a dark theme, which has some example pre-defined styles for some of the more common syntax languages
					string path = ThemesPath + "Dark.vssettings";
					using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
					{
						if (stream != null)
							AmbientHighlightingStyleRegistry.Instance.ImportHighlightingStyles(stream);
					}
				}
			}
		}

	}
}
