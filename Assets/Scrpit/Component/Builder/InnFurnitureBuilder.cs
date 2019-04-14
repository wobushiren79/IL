using UnityEngine;
using UnityEditor;

public class InnFurnitureBuilder : BaseMonoBehaviour
{
    public InnBuildManager innBuildManager;
    public GameDataManager gameDataManager;

    public void BuildFurniture(InnResBean furnitureData)
    {
        if (furnitureData == null)
            return;
        GameObject buildObj= innBuildManager.GetFurnitureObjById(furnitureData.id);
    }
}