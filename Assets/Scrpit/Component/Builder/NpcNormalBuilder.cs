using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcNormalBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //顾客模型
    public GameObject objNormalModel;

    //NPC数据管理
    protected NpcInfoManager npcInfoManager;
    protected NpcTeamManager npcTeamManager;
    protected CharacterBodyManager characterBodyManager;
    protected GameTimeHandler gameTimeHandler;
    protected GameDataManager gameDataManager;
    protected WeatherHandler weatherHandler;

    //初始化大量随机NPC位置
    public List<Transform> listInitStartPosition;
    //有序化生成NPC位置
    public List<Transform> listStartPosition;
    //是否生成NPC
    public bool isBuildNpc = false;
    //生成间隔
    public float buildInterval = 5;
    public int buildMaxNumber = 100;

    protected virtual void Awake()
    {
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager);
        characterBodyManager = Find<CharacterBodyManager>(ImportantTypeEnum.CharacterManager);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        weatherHandler = Find<WeatherHandler>(ImportantTypeEnum.WeatherHandler);
    }

    /// <summary>
    /// 随机获取初始化点位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomInitStartPosition()
    {
        if (CheckUtil.ListIsNull(listInitStartPosition))
            return Vector3.zero;
        Transform initStartPosition = RandomUtil.GetRandomDataByList(listInitStartPosition);
        return GameUtil.GetTransformInsidePosition2D(initStartPosition);
    }

    /// <summary>
    /// 获取开始的随机地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomStartPosition()
    {
        if (CheckUtil.ListIsNull(listStartPosition))
            return Vector3.zero;
        Transform startPosition = RandomUtil.GetRandomDataByList(listStartPosition);
        return GameUtil.GetTransformInsidePosition2D(startPosition);
    }

    /// <summary>
    /// 创建NPC
    /// </summary>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    public GameObject BuildNpc(Vector3 startPosition)
    {
        if (npcInfoManager == null)
            return null;
        CharacterBean characterData = npcInfoManager.GetRandomCharacterData();
        if (characterData == null)
            return null;
        //随机生成身体数据
        characterData.body.CreateRandomBody(characterBodyManager);
        return BuildNpc(characterData, startPosition);
    }

    public GameObject BuildNpc(CharacterBean characterData, Vector3 startPosition)
    {
        return BuildNpc(objNormalModel, characterData, startPosition);
    }

    public GameObject BuildNpc(GameObject objModel, CharacterBean characterData, Vector3 startPosition)
    {
        if (npcInfoManager == null)
            return null;
        if (characterData == null)
            return null;
        GameObject npcObj = Instantiate(objContainer, objModel);
        npcObj.transform.localScale = new Vector3(1, 1);
        npcObj.transform.position = startPosition;

        BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
        baseNpcAI.SetCharacterData(characterData);
        return npcObj;
    }

    /// <summary>
    /// 删除所有NPC
    /// </summary>
    public void ClearNpc()
    {
        CptUtil.RemoveChildsByActive(objContainer.transform);
    }
}