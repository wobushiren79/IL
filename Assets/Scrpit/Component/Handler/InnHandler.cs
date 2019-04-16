using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InnHandler : BaseMonoBehaviour
{
    public enum InnStatusEnum
    {
        Open,
        Close,
    }

    //客栈状态
    public InnStatusEnum innStatus = InnStatusEnum.Open;
    //大门位置
    public Vector3 doorPosition;
    //客栈桌子处理
    public InnTableHandler innTableHandler;
    //排队的人
    public List<NpcAICustomerCpt> cusomerQueue=new List<NpcAICustomerCpt>();


    /// <summary>
    /// 初始化客栈
    /// </summary>
    public void InitInn()
    {
        innTableHandler.InitTableList();
    }

    private void FixedUpdate()
    {
        if (innStatus == InnStatusEnum.Open)
        {
            //排队等待处理
            if (!CheckUtil.ListIsNull(cusomerQueue))
            {
                BuildTableCpt tableCpt = innTableHandler.GetIdleTable();
                if (tableCpt != null)
                {
                    cusomerQueue[0].SetTable(tableCpt);
                    cusomerQueue.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// 加入排队
    /// </summary>
    public void AddQueue(NpcAICustomerCpt customerCpt)
    {
        cusomerQueue.Add(customerCpt);
    }

    /// <summary>
    /// 移除排队
    /// </summary>
    /// <param name="customerCpt"></param>
    public void RemoveQueue(NpcAICustomerCpt customerCpt)
    {
        cusomerQueue.Remove(customerCpt);
    }


}