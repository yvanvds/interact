using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InteractClient.JintEngine.Cocos
{
  public class CCView
  {
    private string LoadGameFunction;
    public CocosSharp.CocosSharpView GameView;

    public CCView(string loadGameFunction)
    {
      GameView = new CocosSharp.CocosSharpView()
      {
        HorizontalOptions = LayoutOptions.FillAndExpand,
        MinimumHeightRequest = 300,
        MinimumWidthRequest = 300,
        VerticalOptions = LayoutOptions.FillAndExpand,
        DesignResolution = new Size(1024, 768),
        ViewCreated = LoadGame,
      };

      GameView.Paused = false;
    }

    void LoadGame(object sender, EventArgs e)
    {
      CocosSharp.CCGameView view = sender as CocosSharp.CCGameView;

      CCScene gameScene = new GameScene(view);
      view.RunWithScene(gameScene);
    }

  }
}
