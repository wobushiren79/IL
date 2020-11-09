using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;

public class NpcAIMiniGameBarrageCpt : BaseNpcAI, SightForMiniGameBarrageCpt.ICallBack
{
    public enum MiniGameBarrageIntentEnum
    {
        Idle = 0,//闲置
        Dodge = 1,//躲避
    }


    //弹幕游戏处理
    protected MiniGameBarrageHandler gameBarrageHandler;
    //迷你游戏数据
    public MiniGameCharacterBean characterMiniGameData;
    //寻路AI
    public NavMeshAgent navMeshAgent;
    //血 粒子
    public ParticleSystem psBlood;
    //视野
    public SightForMiniGameBarrageCpt sightForMiniGameBarrage;


    public override void Awake()
    {
        base.Awake();
        gameBarrageHandler = Find<MiniGameBarrageHandler>( ImportantTypeEnum.MiniGameHandler);
    }

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
        //如果是敌人 则打开AI
        if (characterMiniGameData.characterType == 0)
        {
            OpenAI();
        }
    }

    /// <summary>
    ///遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(int damage)
    {
        if (characterMiniGameData == null
            || gameBarrageHandler == null
            || gameBarrageHandler.GetMiniGameStatus() != MiniGameStatusEnum.Gameing)
            return;
        audioHandler.PlaySound(AudioSoundEnum.Damage);
        characterMiniGameData.AddLife(-damage);
        psBlood.Play();
        //如果是控制的角色并且生命值低于胜利生命值
        if (characterMiniGameData.characterCurrentLife < gameBarrageHandler.miniGameData.winLife)
        {
            if (characterMiniGameData.characterType == 1)
            {
                gameBarrageHandler.EndGame(MiniGameResultEnum.Lose);
            }
        }
    }

    /// <summary>
    /// 打开AI
    /// </summary>
    public void OpenAI()
    {
        navMeshAgent.enabled = true;
        sightForMiniGameBarrage.gameObject.SetActive(true);
        sightForMiniGameBarrage.SetCallBack(this);
    }

    #region 视线回调
    public void SeeBullet(List<MiniGameBarrageBulletCpt> barrageBulletList)
    {
        float distanceTemp = 0;
        Rigidbody2D rbNear = null;
        for (int i = 0; i < barrageBulletList.Count; i++)
        {
            MiniGameBarrageBulletCpt itemBullet = barrageBulletList[i];
            Rigidbody2D rbBullet = itemBullet.GetComponent<Rigidbody2D>();
            //获取最近的子弹
            float distanceItem = Vector2.Distance(rbBullet.position, transform.position);
            if (distanceItem > distanceTemp)
            {
                distanceTemp = distanceItem;
                rbNear = rbBullet;
            }
        }
        Vector3 moveVelocity = new Vector3(-rbNear.velocity.y, rbNear.velocity.x);
        characterMoveCpt.SetDestination(transform.position + moveVelocity.normalized);
    }
    #endregion

}