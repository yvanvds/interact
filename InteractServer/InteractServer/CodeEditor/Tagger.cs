using ActiproSoftware.Text;
using ActiproSoftware.Text.Tagging;
using ActiproSoftware.Text.Tagging.Implementation;
using ActiproSoftware.Text.Utility;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting;
using ActiproSoftware.Windows.Controls.SyntaxEditor.Highlighting.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InteractServer.CodeEditor
{
	public class Tagger : TaggerBase<IClassificationTag>
	{
		private ITagAggregator<ITokenTag> tokenTagAggregator;
		private List<string> tokens = new List<string>();


		static Tagger()
		{
			AmbientHighlightingStyleRegistry.Instance.Register(ClassificationTypes.OtherError, new HighlightingStyle(Brushes.GreenYellow));
		}

		public Tagger(ICodeDocument document) : base("Custom", 
			new Ordering[] {  new Ordering(TaggerKeys.Token, OrderPlacement.Before)}, document)
		{
			// Get a token tag aggregator
			tokenTagAggregator = document.CreateTagAggregator<ITokenTag>();

			tokens.Add("Osc");
			tokens.Add("Log");
			tokens.Add("Client");
		}

		public override IEnumerable<TagSnapshotRange<IClassificationTag>> GetTags(NormalizedTextSnapshotRangeCollection snapshotRanges, object parameter)
		{
			// Loop through the requested snapshot ranges...
			foreach (TextSnapshotRange snapshotRange in snapshotRanges)
			{
				// If the snapshot range is not zero-length...
				if (!snapshotRange.IsZeroLength)
				{
					IEnumerable<TagSnapshotRange<ITokenTag>> tokenTagRanges = tokenTagAggregator.GetTags(snapshotRange);
					if (tokenTagRanges != null)
					{
						foreach (TagSnapshotRange<ITokenTag> tokenTagRange in tokenTagRanges)
						{
							if (tokenTagRange.Tag.Token != null)
							{
								switch (tokenTagRange.Tag.Token.Key)
								{
									case "Identifier":
										{
											// Get the text of the token
											string text = tokenTagRange.SnapshotRange.Text;

											if (tokens.Contains(text))
											{
												// Add a highlighted range
												yield return new TagSnapshotRange<IClassificationTag>(
													new TextSnapshotRange(snapshotRange.Snapshot, tokenTagRange.SnapshotRange.TextRange),
													new ClassificationTag(ClassificationTypes.OtherError)
													);
											}

											break;
										}

								}
							}
						}
					}
				}
			}
		}
	}
}
