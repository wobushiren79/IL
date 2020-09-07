using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class InnResBean
{
    //物品Id
    public long id;
    //起始点
    public Vector3Bean startPosition;
    //物品占地
    public List<Vector3Bean> listPosition;
    //方向
    public int direction;

    //备注ID
    public string remarkId;

    public InnResBean()
    {

    }

    public InnResBean(long id, Vector3 startPosition, List<Vector3> listPosition, Direction2DEnum direction2D)
    {
        this.id = id;
        this.startPosition = new Vector3Bean(startPosition); ;
        if (listPosition != null)
            this.listPosition = TypeConversionUtil.ListV3ToListV3Bean(listPosition);
        this.direction = (int)direction2D;
    }

    public List<Vector3Bean> GetListPosition()
    {
        if (listPosition == null)
            listPosition = new List<Vector3Bean>();
        return listPosition;
    }

    public Vector3 GetStartPosition()
    {
        return TypeConversionUtil.Vector3BeanToVector3(startPosition);
    }
}