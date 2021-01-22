using UnityEditor;
using UnityEngine;

public class InnBuildHandler : BaseHandler<InnBuildHandler, InnBuildManager>
{
    protected InnFurnitureBuilder _builderForFurniture;

    public InnFurnitureBuilder builderForFurniture
    {
        get
        {
            if (_builderForFurniture == null)
            {
                _builderForFurniture = CptUtil.AddCpt<InnFurnitureBuilder>(gameObject);
            }
            return _builderForFurniture;
        }
    }
}