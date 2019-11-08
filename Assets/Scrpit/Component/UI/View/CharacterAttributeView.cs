using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributeView : Graphic
{
    public RectTransform rtfContent;
    public RectTransform rtfThis;

    public int cook;
    public int speed;
    public int account;
    public int charm;
    public int force;


    void Update()
    {
        SetData(cook, speed, account, charm, force);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cook"></param>
    /// <param name="speed"></param>
    /// <param name="account"></param>
    /// <param name="charm"></param>
    /// <param name="force"></param>
    public void SetData(int cook, int speed, int account, int charm, int force)
    {
        this.cook = cook;
        this.speed = speed;
        this.account = account;
        this.charm = charm;
        this.force = force;
        SetAllDirty(); //设置更改，请求渲染（可以在需要的时候手动调用，而不是在update中）
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (rtfContent.childCount == 0)
        {
            return;
        }

        Color32 color32 = color;//绘制的颜色
        vh.Clear(); //清除原先需要绘制的顶点和三角形数据，用下面的元素替代
        float radius = rtfContent.rect.width / 2f;

        // 几何图形的顶点，本例中根据子节点坐标确定顶点
        for (int i = 0; i < rtfContent.childCount; i++)
        {
            RectTransform tfChild = (RectTransform)rtfContent.GetChild(i);
            if (tfChild.name.Contains("Cook"))
            {
                SetAttributePosition(tfChild,i, radius, cook);
            }
            if (tfChild.name.Contains("Speed"))
            {
                SetAttributePosition(tfChild,i, radius, speed);
            }
            if (tfChild.name.Contains("Account"))
            {
                SetAttributePosition(tfChild,i, radius, account);
            }
            if (tfChild.name.Contains("Charm"))
            {
                SetAttributePosition(tfChild,i, radius, charm);
            }
            if (tfChild.name.Contains("Force"))
            {
                SetAttributePosition(tfChild,i, radius, force);
            }
            vh.AddVert(tfChild.localPosition, color32, new Vector2(0f, 0f));
        }
        //几何图形中的三角形
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(0, 3, 4);
        vh.AddTriangle(0, 4, 5);
        vh.AddTriangle(0, 1, 5);
    }

    private void SetAttributePosition(RectTransform tfChild,int position, float radius, int attribute)
    {
        float attributeR = 0;
        if (attribute != 0)
        {
             attributeR = radius / (100f / attribute);
        }
        else
        {
            attributeR = 1;
        }
        float angle = 72 * (position-1);

        float positionY = Mathf.Cos(Mathf.PI * angle / 180) * attributeR;
        float positionX = Mathf.Sin(Mathf.PI * angle / 180) * attributeR;
        tfChild.anchoredPosition = new Vector2(positionX,positionY);
    }
}
