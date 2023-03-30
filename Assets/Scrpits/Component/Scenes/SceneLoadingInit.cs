using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoadingInit : MonoBehaviour
{
    public Image ivLoading;

    public float targetValue;
    private AsyncOperation mOperation;

    public Sprite[] listSpLoading;

    protected int loadingPosition = 0;
    protected float loadingTime = 0;
    protected float loadingTimeScale = 0.05f;

    void Start()
    {
        loadingTime = 0;
        loadingPosition = 0;
        //启动协程
        StartCoroutine(CoroutineForAsyncLoading());
    }

    private void FixedUpdate()
    {
        //if (loadingTime == loadingTimeScale)
        //{
        //    ivLoading.sprite = listSpLoading[loadingPosition];
        //    loadingPosition++;
        //    if (loadingPosition >= listSpLoading.Length)
        //    {
        //        loadingPosition = 0;
        //    }
        //}
        //loadingTime++;
        //if (loadingTime > loadingTimeScale)
        //{
        //    loadingTime = 0;
        //}
    }

    void Update()
    {
        if (loadingTime >= loadingTimeScale)
        {
            loadingTime = 0;
            ivLoading.sprite = listSpLoading[loadingPosition];
            loadingPosition++;
            if (loadingPosition >= listSpLoading.Length)
            {
                loadingPosition = 0;
            }     
        }
        loadingTime += Time.deltaTime;
        //targetValue = mOperation.progress;

        //if (mOperation.progress >= 0.9f)
        //{
        //    //mOperation.progress的值最大为0.9
        //    targetValue = 1.0f;
        //}
        //if (targetValue.Equals(1))
        //{
        //    //允许异步加载完毕后自动切换场景
        //    mOperation.allowSceneActivation = true;
        //}
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineForAsyncLoading()
    {
        yield return new WaitForEndOfFrame();
        Application.backgroundLoadingPriority = ThreadPriority.Low; 
        mOperation = SceneManager.LoadSceneAsync(GameCommonInfo.ScenesChangeData.loadingScene.GetEnumName());
        //阻止当加载完成自动切换
        //mOperation.allowSceneActivation = false;
        yield return mOperation;
    }
}



