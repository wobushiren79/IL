using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class UIPartical : MonoBehaviour
{
    public RectTransform targetUI;
    public float positionZ = 0;
    public void Awake()
    {
        if (targetUI == null)
        {
            targetUI = transform.parent.GetComponent<RectTransform>();
        }
    }

    //更新位置
    public void LateUpdate()
    {
        Vector3 worldPosition = GameUtil.UILocalPointToWorldPoint(targetUI.transform.position, Camera.main, null);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, positionZ);
    }
}
