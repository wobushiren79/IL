using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil {

    public static void SceneChange(string scenenName)
    {
        GameCommonInfo.LoadingSceneName = scenenName;
        SceneManager.LoadSceneAsync(EnumUtil.GetEnumName(ScenesEnum.LoadingScene));
    }



}
