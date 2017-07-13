using ScintillaNET;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InteractServer.Utils;
using Shared;
using InteractServer.Views;
using AutocompleteMenuNS;

namespace InteractServer.Controls
{
  /// <summary>
  /// Interaction logic for CodeEditor.xaml
  /// </summary>
  public partial class CodeEditor : UserControl
  {

    public event EventHandler ContentChanged;
    public bool ServerSide { get; set; } = false;

    private AutocompleteMenu acMenu;
    private Intellisense.AutoComplete ac;

    public CodeEditor()
    {
      InitializeComponent();

      //TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
      TextArea.WrapMode = ScintillaNET.WrapMode.None;
      TextArea.IndentationGuides = ScintillaNET.IndentView.LookBoth;
      TextArea.SetSelectionBackColor(true, Utils.Editor.IntToColor(0x114D9C));
      TextArea.CaretForeColor = System.Drawing.Color.White;
      TextArea.IndentWidth = 2;
      TextArea.TabWidth = 2;
      TextArea.CharAdded += CharAdded;
      TextArea.UseTabs = false;

      InitSyntaxColoring();
      InitNumberMargin();
      InitBookmarkMargin();
      InitCodeFolding();

      acMenu = new AutocompleteMenu();
      acMenu.TargetControlWrapper = new ScintillaWrapper(TextArea);
      acMenu.MaximumSize = new System.Drawing.Size(800, 600);
      acMenu.Colors.BackColor = System.Drawing.Color.Black;
      acMenu.Colors.ForeColor = System.Drawing.Color.DodgerBlue;
      acMenu.Colors.HighlightingColor = System.Drawing.Color.DimGray;
      acMenu.Colors.SelectedBackColor = System.Drawing.Color.DimGray;
      acMenu.Colors.SelectedBackColor2 = System.Drawing.Color.Black;
      acMenu.Colors.SelectedForeColor = System.Drawing.Color.DodgerBlue;

      // for breakpoints and highlighting
      TextArea.Margins[1].Type = MarginType.Symbol;
      TextArea.Markers[0].Symbol = MarkerSymbol.Circle;
      TextArea.Markers[0].SetForeColor(System.Drawing.Color.Red);
      TextArea.Markers[0].SetBackColor(System.Drawing.Color.Red);
      TextArea.Markers[1].Symbol = MarkerSymbol.Background;
      TextArea.Markers[1].SetBackColor(System.Drawing.Color.Red);
    }

    public void InitIntellisense()
    {
      ac = new Intellisense.AutoComplete(TextArea, ServerSide);
      acMenu.SetAutocompleteItems(ac);
    }

    public string ScriptName
    {
      set => ac.ScriptName = value;
      get => ac.ScriptName;
    }

    public string Text
    {
      get
      {
        return TextArea.Text;
      }
      set
      {
        TextArea.Text = value;
      }
    }

    public void HighlightLine(int number, int caretPos = -1)
    {
      TextArea.Lines[number-1].MarkerAdd(1);
      if(caretPos != -1)
      {
        TextArea.GotoPosition(caretPos);
      }
    }

    private void InitSyntaxColoring()
    {

      // Configure the default style
      TextArea.StyleResetDefault();
      TextArea.Styles[ScintillaNET.Style.Default].Font = "Consolas";
      TextArea.Styles[ScintillaNET.Style.Default].Size = 10;
      TextArea.Styles[ScintillaNET.Style.Default].BackColor = Editor.IntToColor(0x212121);
      TextArea.Styles[ScintillaNET.Style.Default].ForeColor = Editor.IntToColor(0xFFFFFF);
      TextArea.StyleClearAll();

      // Configure the CPP (C#) lexer styles
      TextArea.Styles[ScintillaNET.Style.Cpp.Identifier].ForeColor = Editor.IntToColor(0xD0DAE2);
      TextArea.Styles[ScintillaNET.Style.Cpp.Comment].ForeColor = Editor.IntToColor(0xBD758B);
      TextArea.Styles[ScintillaNET.Style.Cpp.CommentLine].ForeColor = Editor.IntToColor(0x40BF57);
      TextArea.Styles[ScintillaNET.Style.Cpp.CommentDoc].ForeColor = Editor.IntToColor(0x2FAE35);
      TextArea.Styles[ScintillaNET.Style.Cpp.Number].ForeColor = Editor.IntToColor(0xFFFF00);
      TextArea.Styles[ScintillaNET.Style.Cpp.String].ForeColor = Editor.IntToColor(0xFFFF00);
      TextArea.Styles[ScintillaNET.Style.Cpp.Character].ForeColor = Editor.IntToColor(0xE95454);
      TextArea.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor = Editor.IntToColor(0x8AAFEE);
      TextArea.Styles[ScintillaNET.Style.Cpp.Operator].ForeColor = Editor.IntToColor(0xE0E0E0);
      TextArea.Styles[ScintillaNET.Style.Cpp.Regex].ForeColor = Editor.IntToColor(0xff00ff);
      TextArea.Styles[ScintillaNET.Style.Cpp.CommentLineDoc].ForeColor = Editor.IntToColor(0x77A7DB);
      TextArea.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = Editor.IntToColor(0x48A8EE);
      TextArea.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor = Editor.IntToColor(0xF98906);
      TextArea.Styles[ScintillaNET.Style.Cpp.CommentDocKeyword].ForeColor = Editor.IntToColor(0xB3D991);
      TextArea.Styles[ScintillaNET.Style.Cpp.CommentDocKeywordError].ForeColor = Editor.IntToColor(0xFF0000);
      TextArea.Styles[ScintillaNET.Style.Cpp.GlobalClass].ForeColor = Editor.IntToColor(0x48A8EE);

      TextArea.Lexer = Lexer.Cpp;

      TextArea.SetKeywords(0, "break case catch class const continue debugger default delete do else export extends finally for function if import in instanceof new return super switch this throw try typeof var void while with yield enum implements interface let package private protected public static await abstract boolean byte char double final float goto int long native short synchronized throws transient volatile null true false");
      TextArea.SetKeywords(1, "");
    }

    #region Numbers, Bookmarks, Code Folding

    /// <summary>
    /// the background color of the text area
    /// </summary>
    private const int BACK_COLOR = 0x2A211C;

    /// <summary>
    /// default text color of the text area
    /// </summary>
    private const int FORE_COLOR = 0xB7B7B7;

    /// <summary>
    /// change this to whatever margin you want the line numbers to show in
    /// </summary>
    private const int NUMBER_MARGIN = 1;

    /// <summary>
    /// change this to whatever margin you want the bookmarks/breakpoints to show in
    /// </summary>
    private const int BOOKMARK_MARGIN = 2;
    private const int BOOKMARK_MARKER = 2;

    /// <summary>
    /// change this to whatever margin you want the code folding tree (+/-) to show in
    /// </summary>
    private const int FOLDING_MARGIN = 3;

    /// <summary>
    /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
    /// </summary>
    private const bool CODEFOLDING_CIRCULAR = true;

    private void InitNumberMargin()
    {

      TextArea.Styles[ScintillaNET.Style.LineNumber].BackColor = Editor.IntToColor(BACK_COLOR);
      TextArea.Styles[ScintillaNET.Style.LineNumber].ForeColor = Editor.IntToColor(FORE_COLOR);
      TextArea.Styles[ScintillaNET.Style.IndentGuide].ForeColor = Editor.IntToColor(FORE_COLOR);
      TextArea.Styles[ScintillaNET.Style.IndentGuide].BackColor = Editor.IntToColor(BACK_COLOR);

      var nums = TextArea.Margins[NUMBER_MARGIN];
      nums.Width = 30;
      nums.Type = MarginType.Number;
      nums.Sensitive = true;
      nums.Mask = 0;

      TextArea.MarginClick += TextArea_MarginClick;
    }

    private void InitBookmarkMargin()
    {

      //TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

      var margin = TextArea.Margins[BOOKMARK_MARGIN];
      margin.Width = 20;
      margin.Sensitive = true;
      margin.Type = MarginType.Symbol;
      margin.Mask = (1 << BOOKMARK_MARKER);
      //margin.Cursor = MarginCursor.Arrow;

      var marker = TextArea.Markers[BOOKMARK_MARKER];
      marker.Symbol = MarkerSymbol.Circle;
      marker.SetBackColor(Editor.IntToColor(0xFF003B));
      marker.SetForeColor(Editor.IntToColor(0x000000));
      marker.SetAlpha(100);

    }

    private void InitCodeFolding()
    {

      TextArea.SetFoldMarginColor(true, Editor.IntToColor(BACK_COLOR));
      TextArea.SetFoldMarginHighlightColor(true, Editor.IntToColor(BACK_COLOR));

      // Enable code folding
      TextArea.SetProperty("fold", "1");
      TextArea.SetProperty("fold.compact", "1");

      // Configure a margin to display folding symbols
      TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
      TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
      TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
      TextArea.Margins[FOLDING_MARGIN].Width = 20;

      // Set colors for all folding markers
      for (int i = 25; i <= 31; i++)
      {
        TextArea.Markers[i].SetForeColor(Editor.IntToColor(BACK_COLOR)); // styles for [+] and [-]
        TextArea.Markers[i].SetBackColor(Editor.IntToColor(FORE_COLOR)); // styles for [+] and [-]
      }

      // Configure folding markers with respective symbols
      TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
      TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
      TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
      TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
      TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
      TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
      TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

      // Enable automatic folding
      TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

    }

    private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
    {
      if (e.Margin == BOOKMARK_MARGIN)
      {
        // Do we have a marker for this line?
        const uint mask = (1 << BOOKMARK_MARKER);
        var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
        if ((line.MarkerGet() & mask) > 0)
        {
          // Remove existing bookmark
          line.MarkerDelete(BOOKMARK_MARKER);
        }
        else
        {
          // Add bookmark
          line.MarkerAdd(BOOKMARK_MARKER);
        }
      }
    }

    #endregion

    private void CharAdded(object sender, CharAddedEventArgs e)
    {

      switch (e.Char)
      {
        case '{':
          {
            // Add closing bracet
            TextArea.InsertText(TextArea.CurrentPosition, " }");
            TextArea.CurrentPosition++;
            break;
          }
        case '[':
          {
            TextArea.InsertText(TextArea.CurrentPosition, "]");
            break;
          }
        case '(':
          {
            TextArea.InsertText(TextArea.CurrentPosition, ")");
            break;
          }
        case ')':
          {
            if(TextArea.GetCharAt(TextArea.CurrentPosition) == ')')
            {
              TextArea.DeleteRange(TextArea.CurrentPosition, 1);
            }
            break;
          }
        case '"':
          {
            TextArea.InsertText(TextArea.CurrentPosition, "\"");
            break;
          }
        case '\'':
          {
            TextArea.InsertText(TextArea.CurrentPosition, "\'");
            break;
          }

        case 13:
          {
            // update intellisense on return
            if (ServerSide)
            {
              Global.IntelliServerScripts.UpdateScript(ac.ScriptName, TextArea.Text);
            } else
            {
              Global.IntelliClientScripts.UpdateScript(ac.ScriptName, TextArea.Text);
            }

            // if the enter key was pressed between { }
            bool endbraces = false;

            if (TextArea.Lines[TextArea.CurrentLine].Text.Equals("}\r\n")) {
              endbraces = true;
            } else if(TextArea.Lines[TextArea.CurrentLine].Text.Equals("}"))
            {
              endbraces = true;
            }

            if(endbraces && TextArea.Lines[TextArea.CurrentLine - 1].Text.EndsWith("{\r\n"))
            {
              // add extra new lines
              TextArea.InsertText(TextArea.CurrentPosition - 3, "\r\n");
              TextArea.InsertText(TextArea.CurrentPosition, "\r\n");

              // change indentation
              TextArea.Lines[TextArea.CurrentLine - 1].Indentation = TextArea.Lines[TextArea.CurrentLine - 2].Indentation;
              TextArea.Lines[TextArea.CurrentLine].Indentation = TextArea.Lines[TextArea.CurrentLine - 2].Indentation + TextArea.TabWidth;
              TextArea.Lines[TextArea.CurrentLine + 1].Indentation = TextArea.Lines[TextArea.CurrentLine - 2].Indentation;

              // move cursor behind added indent
              while (TextArea.GetCharAt(TextArea.CurrentPosition).Equals(' '))
              {
                TextArea.CurrentPosition++;
              }
              // remove text selection which is added when you change the current position
              TextArea.SetEmptySelection(TextArea.CurrentPosition);
            }
            else
            {
              // use the same indentation as the previous line
              TextArea.Lines[TextArea.CurrentLine].Indentation = TextArea.Lines[TextArea.CurrentLine - 1].Indentation;

              // but if that line ended with a brace add an extra indent!
              if (TextArea.Lines[TextArea.CurrentLine - 1].Text.EndsWith("{\r\n"))
              {
                TextArea.Lines[TextArea.CurrentLine].Indentation += TextArea.TabWidth;
              }

              // move cursor to the new indent position
              while (TextArea.GetCharAt(TextArea.CurrentPosition).Equals(' '))
              {
                TextArea.CurrentPosition++;
              }
              // remove text selection which is added when you change the current position
              TextArea.SetEmptySelection(TextArea.CurrentPosition);
            }
            break;
          }

        default:
          {
            break;
          }
      }

    }

    private void TextArea_TextChanged(object sender, EventArgs e)
    {
      TextArea.MarkerDeleteAll(1);

      ContentChanged?.Invoke(this, e);

      int start = TextArea.WordStartPosition(TextArea.CurrentPosition, true);
      char previousChar = (char)TextArea.GetCharAt(start - 1);

      if (TextArea.GetCharAt(TextArea.CurrentPosition - 2) == '.' || previousChar == '.')
      {
        acMenu.MinFragmentLength = 0;
      }
      else if (TextArea.GetWordFromPosition(TextArea.CurrentPosition - 2).Equals("new"))
      {
        acMenu.MinFragmentLength = 0;
      } else
      {
        acMenu.MinFragmentLength = 2;
      }
        
    }
  }

}
