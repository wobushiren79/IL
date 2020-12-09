using UnityEditor;
using UnityEngine;

public class UIGameFamily : UIBaseOne
{
    public GameObject objItemModel;
    public GameObject objContainer;

    public override void OpenUI()
    {
        base.OpenUI();
    }

    public void InitData()
    {
        CptUtil.RemoveChildsByActive(objContainer);    
    }

   
    public void CreateFamilyItem(CharacterBean characterData)
    {
        GameObject objItem =  Instantiate(objContainer, objItemModel);
        ItemGameFamilyCpt itemCpt = objItem.GetComponent<ItemGameFamilyCpt>();
        itemCpt.SetData(characterData);
    }

}