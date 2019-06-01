using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;
    public GameDataManager gameDataManager;

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
    }

    public override void StartControl()
    {
        base.StartControl();
        transform.position = new Vector3(0,0,0);
    }
}