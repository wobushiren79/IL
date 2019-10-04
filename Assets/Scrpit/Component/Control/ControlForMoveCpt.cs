using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;
    public GameDataManager gameDataManager;
    public BaseNpcAI npcAI;

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals(EnumUtil.GetEnumName(ScenesEnum.GameInnScene)))
        {
            cameraFollowObj.transform.position = new Vector3(0, 0);
            characterMoveCpt.minMoveX = -5;
            characterMoveCpt.maxMoveX = gameDataManager.gameData.GetInnBuildData().innWidth + 5;
            characterMoveCpt.minMoveY = -5;
            characterMoveCpt.maxMoveY = gameDataManager.gameData.GetInnBuildData().innHeight + 5;
        }
        InitCharacter();
    }

    public void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis(InputInfo.Horizontal);
        float vMove = Input.GetAxis(InputInfo.Vertical);
        if (hMove == 0 && vMove == 0)
        {
            characterMoveCpt.SetAnimStatus(0);
        }
        else
        {
            characterMoveCpt.Move(hMove, vMove);
        }
    }

    public override void StopControl()
    {
        base.StopControl();
        if (characterMoveCpt != null)
            characterMoveCpt.SetAnimStatus(0);
    }

    public override void EndControl()
    {
        base.EndControl();
        if (characterMoveCpt != null)
            characterMoveCpt.SetAnimStatus(0);
    }

    public override void StartControl()
    {
        base.StartControl();
        InitCharacter();
        //transform.position = new Vector3(0, 0, 0);
    }

    public override void RestoreControl()
    {
        base.RestoreControl();
        InitCharacter();
    }
    /// <summary>
    /// 初始化角色
    /// </summary>
    public void InitCharacter()
    {
        if (gameDataManager != null && gameDataManager.gameData.userCharacter != null && npcAI != null)
            npcAI.SetCharacterData(gameDataManager.gameData.userCharacter);
    }

}