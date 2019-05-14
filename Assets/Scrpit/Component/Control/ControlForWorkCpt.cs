using UnityEngine;
using UnityEditor;

public class ControlForWorkCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt cameraMove;
    public GameDataManager gameDataManager;

    public override void StartControl()
    {
        base.StartControl();
        cameraFollowObj.transform.position = new Vector3(5, 5);
        cameraMove.minMoveX = -1;
        cameraMove.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 1;
        cameraMove.minMoveY = -1;
        cameraMove.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 1;
    }

    private void FixedUpdate()
    {
        if (cameraMove == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if (hMove == 0 && vMove == 0)
        {
            cameraMove.StopAnim();
        }
        else
        {
            cameraMove.Move(hMove, vMove);
        }
    }
}