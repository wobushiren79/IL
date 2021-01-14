using UnityEngine;
using UnityEditor;

public class GameUtil
{
    /// <summary>
    /// 获取tf里随机一个点
    /// </summary>
    /// <param name="tfTarget"></param>
    /// <returns></returns>
    /// 
    public static Vector3 GetTransformInsidePosition2D(Transform tfTarget)
    {
        return GetTransformInsidePosition2D(tfTarget, 1);
    }
    public static Vector3 GetTransformInsidePosition2D(Transform tfTarget, float size)
    {
        if (tfTarget == null)
            return Vector3.zero;
        float tempX = tfTarget.localScale.x / 2f;
        float tempY = tfTarget.localScale.y / 2f;
        //修正避免太靠边
        //tempX -= 0.5f;
        //tempY -= 0.5f;

        float randomXoff = Random.Range(-tempX, tempX);
        float randomYoff = Random.Range(-tempY, tempY);
        return new Vector3(tfTarget.position.x + randomXoff * size, tfTarget.position.y + randomYoff * size);
    }

    /// <summary>
    /// 获取屏幕宽
    /// </summary>
    /// <returns></returns>
    public static float GetScreenWith()
    {
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        return rightBorder - leftBorder;
    }
    /// <summary>
    /// 获取屏幕高
    /// </summary>
    /// <returns></returns>
    public static float GetScreenHeight()
    {
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        return topBorder - downBorder;
    }

    /// <summary>
    /// 刷新UI控件高
    /// </summary>
    /// <param name="itemRTF"></param>
    /// <param name="isWithFitter">宽是否自适应大小</param>
    public static void RefreshRectViewHight(RectTransform itemRTF, bool isWithFitter)
    {
        if (itemRTF == null)
            return;
        float itemWith = itemRTF.rect.width;
        if (isWithFitter)
        {
            itemWith = 0;
        }
        float itemHight = itemRTF.rect.height;
        RectTransform[] childTFList = itemRTF.GetComponentsInChildren<RectTransform>();
        if (childTFList == null)
            return;
        itemHight = 0;
        foreach (RectTransform itemTF in childTFList)
        {
            itemHight += itemTF.rect.height;
        }
        //设置大小
        if (itemRTF != null)
            itemRTF.sizeDelta = new Vector2(itemWith, itemHight);

    }

    /// <summary>
    /// 世界坐标转换为本地UI坐标
    /// 成功转化的前提条件为UI所用摄像头为Camera.main
    /// </summary>
    /// <param name="rtfCanvas"></param>
    /// <param name="camera"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static Vector2 WorldPointToUILocalPoint(RectTransform rtfCanvas, Vector3 worldPositon)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPositon);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rtfCanvas, screenPoint, Camera.main, out Vector2 localPoint);
        return localPoint;
    }
    public static void WorldPointToUILocalPoint(RectTransform uiParent, Vector3 worldPositon,  RectTransform uiTarget)
    {
        uiTarget.anchoredPosition = WorldPointToUILocalPoint(uiParent, worldPositon);
    }



    /// <summary>
    /// 鼠标位置转为屏幕UGUI位置
    /// </summary>
    /// <param name="camera">如果画布是screenspace-overlay 则直接设置相机为NULL</param>
    /// <param name="uiParent"></param>
    /// <returns></returns>
    public static Vector2 MousePointToUGUIPoint(Camera camera, RectTransform uiParent)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiParent, Input.mousePosition, camera, out Vector2 vecMouse);
        return vecMouse;
    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }


}