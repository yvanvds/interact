using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.JintEngine.Cocos
{
  public class GameScene : CCScene
  {
    static readonly CCColor4B backgroundColor = new CCColor4B(88, 140, 238);
    CCLayer mainLayer;

    public GameScene(CCGameView gameView) : base(gameView)
    {
      mainLayer = new CCLayerColor(backgroundColor);
      this.AddLayer(mainLayer);
    }
  }

}
