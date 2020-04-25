using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterAttributeView : Graphic
{
    public RectTransform rtfContent;
    public RectTransform rtfThis;

    public Text tvLucky;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;

    public int lucky;
    public int cook;
    public int speed;
    public int account;
    public int charm;
    public int force;

    //是否在初始化中
    private bool mIsIniting = false;
    //private void Update()
    //{
    //   
    //        SetAllDirty(); //设置更改，请求渲染（可以在需要的时候手动调用，而不是在update中）
    //}

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="characterAttributes"></param>
    public void SetData(CharacterAttributesBean characterAttributes)
    {
        SetData(characterAttributes.cook, characterAttributes.speed, characterAttributes.account, characterAttributes.charm, characterAttributes.force, characterAttributes.lucky);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cook"></param>
    /// <param name="speed"></param>
    /// <param name="account"></param>
    /// <param name="charm"></param>
    /// <param name="force"></param>
    public void SetData(int cook, int speed, int account, int charm, int force, int lucky)
    {
        this.cook = cook;
        this.speed = speed;
        this.account = account;
        this.charm = charm;
        this.force = force;
        this.lucky = lucky;

        if (tvCook != null)
            tvCook.text = "(" + cook + ")" + GameCommonInfo.GetUITextById(1);
        if (tvSpeed != null)
            tvSpeed.text = "(" + speed + ")" + GameCommonInfo.GetUITextById(2);
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "(" + account + ")";
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "(" + charm + ")";
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "(" + force + ")";
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "(" + force + ")";
        StartCoroutine(CoroutineForInit());
    }

    public IEnumerator CoroutineForInit()
    {
        mIsIniting = true;
        yield return new WaitForEndOfFrame();
        //设置每个点的位置
        InitPosition();
        for (int i = 0; i < rtfContent.childCount; i++)
        {
            RectTransform tfChild = (RectTransform)rtfContent.GetChild(i);
            tfChild.DOAnchorPos(Vector2.zero, 2)
                .From()
                .SetEase(Ease.OutCubic)
                .OnUpdate(delegate ()
                {
                    SetAllDirty();
                })
                .OnComplete(delegate ()
                {
                    mIsIniting = false;
                });
        }
    }

    private void Update()
    {
        if (!mIsIniting)
        {
            InitPosition();
            SetAllDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (rtfContent.childCount == 0)
        {
            return;
        }
        Color32 color32 = color;//绘制的颜色
        vh.Clear(); //清除原先需要绘制的顶点和三角形数据，用下面的元素替代

        // 几何图形的顶点，本例中根据子节点坐标确定顶点
        for (int i = 0; i < rtfContent.childCount; i++)
        {
            RectTransform tfChild = (RectTransform)rtfContent.GetChild(i);
            vh.AddVert(tfChild.localPosition, color32, new Vector2(0f, 0f));
        }
        //几何图形中的三角形
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(0, 3, 4);
        vh.AddTriangle(0, 4, 5);
        vh.AddTriangle(0, 5, 6);
        vh.AddTriangle(0, 1, 6);
    }

    /// <summary>
    /// 初始化点的位置
    /// </summary>
    private void InitPosition()
    {
        float minWidth = 0;
        if (rtfContent.rect.width < rtfContent.rect.height)
        {
            minWidth = rtfContent.rect.width;
        }
        else
        {
            minWidth = rtfContent.rect.height;
        }
        float radius = minWidth / 2f;
        for (int i = 0; i < rtfContent.childCount; i++)
        {
            RectTransform tfChild = (RectTransform)rtfContent.GetChild(i);
            if (tfChild.name.Contains("Lucky"))
            {
                SetAttributePosition(tfChild, i, radius, lucky);
            }
            if (tfChild.name.Contains("Cook"))
            {
                SetAttributePosition(tfChild, i, radius, cook);
            }
            if (tfChild.name.Contains("Speed"))
            {
                SetAttributePosition(tfChild, i, radius, speed);
            }
            if (tfChild.name.Contains("Account"))
            {
                SetAttributePosition(tfChild, i, radius, account);
            }
            if (tfChild.name.Contains("Charm"))
            {
                SetAttributePosition(tfChild, i, radius, charm);
            }
            if (tfChild.name.Contains("Force"))
            {
                SetAttributePosition(tfChild, i, radius, force);
            }
        }
    }

    /// <summary>
    /// 设置每个点的位置
    /// </summary>
    /// <param name="tfChild"></param>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <param name="attribute"></param>
    private void SetAttributePosition(RectTransform tfChild, int position, float radius, int attribute)
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
        float angle = 60 * (position - 1);

        float positionY = Mathf.Cos(Mathf.PI * angle / 180) * attributeR;
        float positionX = Mathf.Sin(Mathf.PI * angle / 180) * attributeR;
        tfChild.anchoredPosition = new Vector2(positionX, positionY);
    }
}
