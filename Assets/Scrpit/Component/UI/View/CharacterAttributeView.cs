using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributeView : Graphic
{
    void Update()
    {
        SetAllDirty(); //设置更改，请求渲染（可以在需要的时候手动调用，而不是在update中）
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (transform.childCount == 0)
        {
            return;
        }

        Color32 color32 = color;//绘制的颜色
        vh.Clear(); //清除原先需要绘制的顶点和三角形数据，用下面的元素替代

        // 几何图形的顶点，本例中根据子节点坐标确定顶点
        int index = 0;
        foreach (Transform child in transform)
        {
            vh.AddVert(child.localPosition, color32, new Vector2(0f, 0f));
        }
        //几何图形中的三角形
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(0, 3, 4);
    }
}
