using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectView : BaseMonoBehaviour
{
    public Button leftSelect;
    public Button rightSelect;

    public Text tvPosition;//选择序号

    private int itemPosition = 0;
    private int listDataNumber = 0;
    private CallBack callBack;

    private void Start()
    {
        if (leftSelect != null)
            leftSelect.onClick.AddListener(LeftSelect);
        if (rightSelect != null)
            rightSelect.onClick.AddListener(RightSelect);
    }

    public void SetCallBack(CallBack callBack)
    {
        this.callBack = callBack;
    }

    public void LeftSelect()
    {
        if (listDataNumber == 0)
        {
            return;
        }
        if (itemPosition - 1 < 0 )
        {
            itemPosition = listDataNumber - 1;
        }
        else
        {
            itemPosition -= 1;
        }
        SetPosition(itemPosition);
    }

    public void RightSelect()
    {
        if (listDataNumber==0)
        {
            return;
        }
        if ((itemPosition + 1) >= listDataNumber)
        {
            itemPosition = 0;
        }
        else
        {
            itemPosition += 1;
        }
        SetPosition(itemPosition);
    }

    public void SetPosition(int position)
    {
        itemPosition = position;
        tvPosition.text = (itemPosition+1) + "";
        if (callBack != null)
        {
            callBack.ChangeSelectPosition(this,position);
        }
    }

    public void SetSelectNumber(int listDataNumber)
    {
        this.listDataNumber = listDataNumber;
        SetPosition(0);
    }
    
    /// <summary>
    /// 获取选取的数据
    /// </summary>
    /// <returns></returns>
    public int GetSelectPosition()
    {
        if (listDataNumber==0)
        {
            return 0;
        }
        else
        {
            return itemPosition;
        }
       
    }

    public interface CallBack
    {
        void ChangeSelectPosition(SelectView selectView, int position);
    }
}