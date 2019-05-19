using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;
    public GameDataManager gameDataManager;
    //角色互动
    public CharacterInteractiveCpt characterInteractiveCpt;

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals("GameInnScene"))
        {
            cameraFollowObj.transform.position = new Vector3(0, 0);
            characterMoveCpt.minMoveX = -5;
            characterMoveCpt.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 5;
            characterMoveCpt.minMoveY = -5;
            characterMoveCpt.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 5;
        }
    }

    private void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if (hMove == 0 && vMove == 0)
        {
            characterMoveCpt.StopAnim();
        }
        else
        {
            characterMoveCpt.Move(hMove, vMove);
        }
        if(transform.position.x<= characterMoveCpt.minMoveX
            || transform.position.x >= characterMoveCpt.maxMoveX
              || transform.position.y <= characterMoveCpt.minMoveY
                || transform.position.y >= characterMoveCpt.maxMoveY)
        {
            characterInteractiveCpt.SetChangeLocation("进入小镇");
        }
        else
        {
            characterInteractiveCpt.CloseInteractive();
        }
    }

    public override void StartControl()
    {
        base.StartControl();
        transform.position = new Vector3(0,0,0);
    }
}