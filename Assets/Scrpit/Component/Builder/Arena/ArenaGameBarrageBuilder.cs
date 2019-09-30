using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ArenaGameBarrageBuilder : BaseMonoBehaviour
{
    //发射器容器
    public GameObject objEjectorContainer;
    //发射器模型
    public GameObject objEjectorModel;
    //发射器创建位置
    public Transform tfEjectorPosition;

    //所有的发射器
    public List<BarrageEjectorCpt> listEjector = new List<BarrageEjectorCpt>();

    /// <summary>
    /// 获取所有的发射器
    /// </summary>
    /// <returns></returns>
    public List<BarrageEjectorCpt> GetEjector()
    {
        return listEjector;
    }

    /// <summary>
    /// 创建一个发射器
    /// </summary>
    /// <param name="position"></param>
    public BarrageEjectorCpt CreateEjector()
    {
        GameObject objEjector = Instantiate(objEjectorModel, objEjectorContainer.transform);
        objEjector.SetActive(true);
        objEjector.transform.position = tfEjectorPosition.position;
        BarrageEjectorCpt ejectorCpt = objEjector.GetComponent<BarrageEjectorCpt>();
        listEjector.Add(ejectorCpt);
        return ejectorCpt;
    }

    /// <summary>
    /// 清理所有的发射器
    /// </summary>
    public void DestoryEjector()
    {
        CptUtil.RemoveChild(objEjectorContainer.transform);
        listEjector.Clear();
    }


}