﻿using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;

    //走路声音
    public AudioSource audioForWalk;
    public BaseNpcAI npcAI;

    //是否控制移动
    protected bool isControlForMove = false;


    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals(ScenesEnum.GameInnScene.GetEnumName()))
        {
            //characterMoveCpt.minMoveX = -5;
            //characterMoveCpt.maxMoveX = gameData.GetInnBuildData().innWidth + 5;
            //characterMoveCpt.minMoveY = -5;
            //characterMoveCpt.maxMoveY = gameData.GetInnBuildData().innHeight + 5;
        }
        InitCharacter();
    }

    private void OnEnable()
    {
        SetCameraOrthographicSize();
    }

    private void OnDisable()
    {
        if (audioForWalk != null && audioForWalk.isPlaying)
        {
            audioForWalk.Stop();
        }
    }


    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis(InputInfo.Horizontal);
        float vMove = Input.GetAxis(InputInfo.Vertical);
        if (hMove == 0 && vMove == 0)
        {
            isControlForMove = false;
            characterMoveCpt.SetAnimStatus(0);
            if (audioForWalk != null && audioForWalk.isPlaying)
            {
                audioForWalk.Stop();
            }
        }
        else
        {
            isControlForMove = true;
            characterMoveCpt.Move(hMove, vMove);
            if (audioForWalk != null && !audioForWalk.isPlaying)
            {
                audioForWalk.Play();
            }
        }
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
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
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Rest);
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (gameData.userCharacter != null && npcAI != null && npcAI as NpcAIUserCpt)
        {
            npcAI.SetCharacterData(gameData.userCharacter);
      
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            audioForWalk.volume = gameConfig.soundVolume * 0.5f;

            audioForWalk.clip = LoadAddressablesUtil.LoadAssetSync<AudioClip>("Assets/Audio/Sound/sound_walk_1.mp3");
        }
    }

}