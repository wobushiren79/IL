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

    protected InnFloorBuilder _builderForFloor;
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

    protected InnWallBuilder _builderForWall;
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

    //后庭建设
    protected InnCourtyardBuilder _builderForCourtyard;
    public InnCourtyardBuilder builderForCourtyard
    {
        get
        {
            if (_builderForCourtyard == null)
            {
                _builderForCourtyard = CptUtil.AddCpt<InnCourtyardBuilder>(gameObject);
            }
            return _builderForCourtyard;
        }
    }
}