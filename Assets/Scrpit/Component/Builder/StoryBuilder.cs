using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class StoryBuilder : BaseMonoBehaviour, StoryInfoManager.CallBack,UIGameText.CallBack
{
    [Header("控件")]
    public GameObject objNpcModle;

    [Header("数据")]
    public StoryInfoManager storyInfoManager;
    public NpcInfoManager npcInfoManager;
    public BaseUIManager uiManager;

    public StoryInfoBean storyInfo;
    public List<StoryInfoDetailsBean> listStoryDetails;
    public List<GameObject> listNpcObj;

    public int storyOrder = 1;

    private void Awake()
    {
        listNpcObj = new List<GameObject>();
    }


    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        storyInfoManager.GetStoryDetailsById(storyInfo.id, this);
    }

    public void GetStoryDetailsSuccess(List<StoryInfoDetailsBean> listData)
    {
        ClearStoryScene();
        listStoryDetails = listData;
        storyOrder = 1;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(storyOrder);
        CreateStoryScene(listOrderData);
    }

    /// <summary>
    /// 根据顺序获取故事
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public List<StoryInfoDetailsBean> GetStoryDetailsByOrder(int order)
    {
        List<StoryInfoDetailsBean> listData = new List<StoryInfoDetailsBean>();
        if (listStoryDetails == null)
            return listData;

        foreach (StoryInfoDetailsBean itemData in listStoryDetails)
        {
            if (itemData.order == order)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建故事场景
    /// </summary>
    public void CreateStoryScene(List<StoryInfoDetailsBean> listData)
    {
        if (storyInfo == null)
            return;
        //设置剧情发生坐标
        gameObject.transform.position = new Vector3(storyInfo.position_x, storyInfo.position_y);
        foreach (StoryInfoDetailsBean itemData in listData)
        {
            switch (itemData.type)
            {
                case 1:
                    //Npc站位
                    GameObject objNpc = GetNpcByNpcNum(itemData.npc_num);
                    if (objNpc == null)
                        CreateNpc(itemData);
                    else
                    {
                        BaseNpcAI npcAI = objNpc.GetComponent<BaseNpcAI>();
                        npcAI.characterMoveCpt.SetDestination(new Vector3(itemData.npc_position_x,itemData.npc_position_y));
                    }
                    break;
                case 11:
                    UIGameText uiComponent = (UIGameText)uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
                    uiComponent.SetCallBack(this);
                    uiComponent.SetData(TextEnum.Story, itemData.text_mark_id);  
                    break;
                case 12:
                    //剧情自动跳转
                    StartCoroutine(StoryAutoNext(itemData.wait_time));
                    break;
            }
        }
    }

    public IEnumerator StoryAutoNext(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextOrder();
    } 

    /// <summary>
    /// 通过Npc编号获取NPC
    /// </summary>
    /// <param name="npcNum"></param>
    /// <returns></returns>
    public GameObject GetNpcByNpcNum(int npcNum)
    {
        if (listNpcObj != null)
        {
            foreach (GameObject itemNpc in listNpcObj)
            {
                if (itemNpc.name.Equals(npcNum + ""))
                {
                    return itemNpc;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 生成NPC
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateNpc(StoryInfoDetailsBean itemData)
    {
        GameObject objNpc = Instantiate(objNpcModle, transform);
        objNpc.SetActive(true);
        objNpc.transform.localPosition = new Vector3(itemData.npc_position_x, itemData.npc_position_y);
        listNpcObj.Add(objNpc);
        NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemData.npc_id);
        //设置编号
        objNpc.name = itemData.npc_num + "";

        aiNpc.SetCharacterData(characterData);
    }

    /// <summary>
    /// 清理剧情场景
    /// </summary>
    public void ClearStoryScene()
    {
        CptUtil.RemoveChildsByActive(transform);
        gameObject.transform.position = Vector3.zero;
        listNpcObj.Clear();
    }

    /// <summary>
    /// 下一个剧情点
    /// </summary>
    public void NextOrder()
    {
        storyOrder++;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(storyOrder);
        if (CheckUtil.ListIsNull(listOrderData))
        {
            //没有剧情。完结
            EventHandler.Instance.isEventing = false;
            uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
            ClearStoryScene();
        }
        else
        CreateStoryScene(listOrderData);
    }

    #region 文本结束回调
    public void TextEnd()
    {
        NextOrder();
    }
    #endregion
}