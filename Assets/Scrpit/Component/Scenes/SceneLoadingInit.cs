using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingInit : MonoBehaviour
{
    private float targetValue;
    private AsyncOperation mOperation;

    // Use this for initialization
    void Start()
    {
        //启动协程
        StartCoroutine(AsyncLoading());
       // StartCoroutine(prepareTime());
    }

    // Update is called once per frame
    void Update()
    {
        targetValue = mOperation.progress;

        if (mOperation.progress >= 0.9f)
        {
            //mOperation.progress的值最大为0.9
            targetValue = 1.0f;
        }
        if (targetValue.Equals(1))
        {
            //允许异步加载完毕后自动切换场景
            mOperation.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoading()
    {
        mOperation = SceneManager.LoadSceneAsync(GameCommonInfo.LoadingSceneName);
        //阻止当加载完成自动切换
        mOperation.allowSceneActivation = false;
        yield return mOperation;
    }
}



