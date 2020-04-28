using UnityEngine;
using UnityEditor;

public class SceneGameMountainInit : BaseNormalSceneInit
{

    public override void Start()
    {
        base.Start();
    }


    public override ControlForMoveCpt InitUserPosition()
    {
        ControlForMoveCpt controlForMove =  base.InitUserPosition();

        return controlForMove;
    }

}