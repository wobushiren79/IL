using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcNormalBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //顾客模型
    public GameObject objNormalModel;

    //初始化大量随机NPC位置
    public List<Transform> listInitStartPosition;
    //有序化生成NPC位置
    public List<Transform> listStartPosition;
    //是否生成NPC
    public bool isBuildNpc = false;
    //生成间隔
    public float buildInterval = 5;
    public int buildMaxNumber = 100;

    public Queue<GameObject> listHideNpc = new Queue<GameObject>();


    protected virtual void Awake()
    {

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
        return BuildNpc(listHideNpc, objNormalModel, startPosition);
    }

    public GameObject BuildNpc(Queue<GameObject> listHide,GameObject objNpcModel, Vector3 startPosition)
    {
        CharacterBean characterData = NpcInfoHandler.Instance.manager.GetRandomCharacterData();
        if (characterData == null)
            return null;
        //随机生成身体数据
        characterData.body.CreateRandomBody();
        return BuildNpc(listHide, objNpcModel, characterData, startPosition);
    }

    public GameObject BuildNpc(Queue<GameObject> listHide, GameObject objModel, CharacterBean characterData, Vector3 startPosition)
    {
        if (characterData == null || characterData.body == null)
            return null;

        GameObject npcObj = null;

        if (listHide.Count > 0)
        {
            npcObj = listHide.Dequeue();
            if (npcObj == null)
            {
                return BuildNpc(listHide, objModel, characterData, startPosition);
            }         
        }
        else
        {        
            npcObj = Instantiate(objContainer, objModel);
        }
        if(!npcObj.activeSelf)
            npcObj.gameObject.SetActive(true);
        npcObj.transform.localScale = new Vector3(1, 1);
        npcObj.transform.position = startPosition;

        BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
        baseNpcAI.ClearAllStatusIcon();

        //如果没有眼睛
        if (CheckUtil.StringIsNull(characterData.body.eye))
        {
            //随机生成身体数据
            characterData.body.CreateRandomEye();
        }
        baseNpcAI.SetCharacterData(characterData);
        return npcObj;
    }

    /// <summary>
    /// 删除所有NPC
    /// </summary>
    public virtual void ClearNpc()
    {
        CptUtil.RemoveChildsByActive(objContainer.transform);
    }
}