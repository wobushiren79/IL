using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil
{

    public static void SceneChange(ScenesEnum scenenName)
    {
        //获取当前场景名字
        string beforeSceneName = SceneManager.GetActiveScene().name;
        GameCommonInfo.ScenesChangeData.beforeScene = EnumUtil.GetEnum<ScenesEnum>(beforeSceneName);
        GameCommonInfo.ScenesChangeData.loadingScene = scenenName;
        //SceneManager.LoadSceneAsync(EnumUtil.GetEnumName(ScenesEnum.LoadingScene));
        SceneManager.LoadScene(EnumUtil.GetEnumName(ScenesEnum.LoadingScene));
    }

    /// <summary>
    /// 获取当前场景
    /// </summary>
    /// <returns></returns>
    public static ScenesEnum GetCurrentScene()
    {
        //获取当前场景名字
        string sceneName = SceneManager.GetActiveScene().name;
        return EnumUtil.GetEnum<ScenesEnum>(sceneName);
    }

}
