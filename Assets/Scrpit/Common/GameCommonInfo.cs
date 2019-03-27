using UnityEngine;
using UnityEditor;

public class GameCommonInfo 
{
    public static  GameConfigBean gameConfig;

    private static GameConfigController mGameConfigController;
    private static UITextController mUITextController;
     
    static GameCommonInfo()
    {
        gameConfig = new GameConfigBean();
        mGameConfigController = new GameConfigController(null, new GameConfigCallBack());
        mUITextController = new UITextController(null,null);
        mGameConfigController.GetGameConfigData();
    }


    public static string GetUITextById(long id)
    {
       return mUITextController.GetTextById(id);
    }


    public class GameConfigCallBack : IGameConfigView
    {
        public void GetGameConfigFail()
        {

        }

        public void GetGameConfigSuccess(GameConfigBean configBean)
        {
            gameConfig = configBean;
            mUITextController.RefreshData();
        }

        public void SetGameConfigFail()
        {

        }

        public void SetGameConfigSuccess(GameConfigBean configBean)
        {

        }
    }
}