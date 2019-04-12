using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ItemBean 
{
    public long itemId;
    public long itemNumber;

    public ItemBean()
    {

    }
    public ItemBean(long id,long number)
    {
        itemId = id;
        itemNumber = number;
    }
}