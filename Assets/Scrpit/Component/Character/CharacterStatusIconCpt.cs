using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class CharacterStatusIconCpt : BaseMonoBehaviour
{
    public GameObject objIconModel;

    public List<CharacterStatusIconItemCpt> listStatusIcon = new List<CharacterStatusIconItemCpt>();

    public void AddStatusIcon(CharacterStatusIconBean statusData)
    {
        //获取新的位置
        float totalX = (listStatusIcon.Count) * 0.5f;
        float startX = -(totalX / 2f);
        //设置新的位置
        for (int i = 0; i < listStatusIcon.Count; i++)
        {
            CharacterStatusIconItemCpt itemCpt = listStatusIcon[i];
            itemCpt.transform.DOLocalMoveX(startX + i * 0.5f, 0.5f);
        }
        //创建添加的图标
        CreateStatusIcon(statusData, new Vector3(startX + listStatusIcon.Count * 0.5f, objIconModel.transform.localPosition.y));
    }

    /// <summary>
    /// 创建图标
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="iconPosition"></param>
    /// <returns></returns>
    public GameObject CreateStatusIcon(CharacterStatusIconBean itemData, Vector3 iconPosition)
    {
        GameObject objStatus = Instantiate(gameObject, objIconModel);
        objStatus.transform.localPosition = iconPosition;
        CharacterStatusIconItemCpt itemCpt = objStatus.GetComponent<CharacterStatusIconItemCpt>();
        itemCpt.SetData(itemData);
        listStatusIcon.Add(itemCpt);
        objStatus.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack);
        return objStatus;
    }

    /// <summary>
    /// 根据类型移除图标
    /// </summary>
    /// <param name="characterStatus"></param>
    public void RemoveStatusIconByType(CharacterStatusIconEnum characterStatus)
    {
        for (int i = 0; i < listStatusIcon.Count; i++)
        {
            CharacterStatusIconItemCpt itemData = listStatusIcon[i];
            if (itemData.statusIconData.iconStatus == characterStatus)
            {
                itemData.transform.DOScale(new Vector3(0, 0, 0), 0.5f).OnComplete(delegate { Destroy(itemData.gameObject); });
                listStatusIcon.Remove(itemData);
                i--;
            }
        }
        float totalX = (listStatusIcon.Count - 1) * 0.5f;
        float startX = -(totalX / 2f);
        //设置新的位置
        for (int i = 0; i < listStatusIcon.Count; i++)
        {
            CharacterStatusIconItemCpt itemCpt = listStatusIcon[i];
            itemCpt.transform.DOLocalMoveX(startX + i * 0.5f, 0.5f);
        }
    }

    /// <summary>
    /// 修改图标
    /// </summary>
    /// <param name="statusData"></param>
    public void ChangeStatusIcon(CharacterStatusIconBean statusData)
    {
        bool hasData = false;
        for (int i = 0; i < listStatusIcon.Count; i++)
        {
            CharacterStatusIconItemCpt itemData = listStatusIcon[i];
            if (itemData.statusIconData.iconStatus == statusData.iconStatus)
            {
                hasData = true;
                itemData.statusIconData.spIcon = statusData.spIcon;
                itemData.statusIconData.spColor = statusData.spColor;
                itemData.SetData(itemData.statusIconData);
                itemData.transform.DOScale(new Vector3(0,0,0),0.5f).From().SetEase(Ease.OutBack); ;
            }
        }
        if (!hasData)
        {
            AddStatusIcon(statusData);
        }
    }

}

public enum CharacterStatusIconEnum
{
    Mood = 1,//心情
    NpcType = 2,//Npc类型
}

public class CharacterStatusIconBean
{
    public CharacterStatusIconEnum iconStatus;
    public Sprite spIcon;
    public Color spColor = Color.white;
}