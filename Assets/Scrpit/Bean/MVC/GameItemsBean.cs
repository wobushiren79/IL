using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GameItemsBean : ItemBean
{
    public long priceL;
    public long priceM;
    public long priceS;

    public GameItemsBean(long id) : base(id, 0)
    {
    }

    public GameItemsBean(long id, long number) : base(id, number)
    {
    }

    public GameItemsBean(long id, long number, long priceL, long priceM, long priceS) : base(id, number)
    {
        this.priceL = priceL;
        this.priceM = priceM;
        this.priceS = priceS;
    }
}