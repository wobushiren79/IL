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
    private  List<IconBean> listData;
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
        if (CheckUtil.ListIsNull(listData)) {
            return;
        }
        if (itemPosition - 1 < 0 )
        {
            itemPosition = listData.Count - 1;
        }
        else
        {
            itemPosition -= 1;
        }
        SetPosition(itemPosition);
    }

    public void RightSelect()
    {
        if (CheckUtil.ListIsNull(listData))
        {
            return;
        }
        if ((itemPosition + 1) >= listData.Count)
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
            callBack.ChangeSelectPosition(this,position, listData[position]);
        }
    }

    public void SetSelectData(List<IconBean> listData)
    {
        this.listData = listData;
        SetPosition(0);
    }
    
    /// <summary>
    /// 获取选取的数据
    /// </summary>
    /// <returns></returns>
    public IconBean GetSelectData()
    {
        if (CheckUtil.ListIsNull(listData))
        {
            return null;
        }
        else
        {
            return listData[itemPosition];
        }
       
    }

    public interface CallBack
    {
        void ChangeSelectPosition(SelectView selectView, int position, IconBean iconBean);
    }
}