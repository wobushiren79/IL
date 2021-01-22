using UnityEditor;
using UnityEngine;

public class InnBuildHandler : BaseHandler<InnBuildHandler, InnBuildManager>
{
    protected InnFurnitureBuilder _builderForFurniture;
    protected InnFloorBuilder _builderForFloor;
    protected InnWallBuilder _builderForWall;
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

    public InnFloorBuilder builderForFloor
    {
        get
        {
            if (_builderForFloor == null)
            {
                _builderForFloor = CptUtil.AddCpt<InnFloorBuilder>(gameObject);
            }
            return _builderForFloor;
        }
    }

    public InnWallBuilder builderForWall
    {
        get
        {
            if (_builderForWall == null)
            {
                _builderForWall = CptUtil.AddCpt<InnWallBuilder>(gameObject);
            }
            return _builderForWall;
        }
    }
}