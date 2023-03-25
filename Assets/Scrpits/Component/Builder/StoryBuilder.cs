using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class StoryBuilder : BaseMonoBehaviour
{
    //剧情
    public StoryInfoBean storyInfo;
    //剧情详情
    public List<StoryInfoDetailsBean> listStoryDetails;
    //剧情NPC列表
    public List<GameObject> listNpcObj;
    //剧情道具列表
    public List<GameObject> listPropObj;

    //剧情点
    private int mStoryOrder = 1;
    //迷你游戏数据
    private MiniGameCookingBean mGameCookingData;


    public virtual void Awake()
    {
        listStoryDetails = new List<StoryInfoDetailsBean>();
        listNpcObj = new List<GameObject>();
        listPropObj = new List<GameObject>();
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    /// <param name="listData"></param>
    public void BuildStory(StoryInfoBean storyInfo)
    {
        this.storyInfo = storyInfo;
        StoryInfoHandler.Instance.manager.GetStoryDetailsById(storyInfo.id, SetStoryDetailsData);
    }

    /// <summary>
    /// 创建迷你烹饪游戏的故事
    /// </summary>
    /// <param name="storyInfo"></param>
    /// <param name="gameCookingData"></param>
    public void BuildStoryForMiniGameCooking(StoryInfoBean storyInfo, MiniGameCookingBean gameCookingData)
    {
        mGameCookingData = gameCookingData;
        BuildStory(storyInfo);
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
            if (itemData.story_order == order)
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
        bool isNext = true;
        for (int i = 0; i < listData.Count; i++)
        {
            StoryInfoDetailsBean itemData = listData[i];
            switch (itemData.GetStoryInfoDetailsType())
            {
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcPosition:
                    HandleForNpcPosition(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcDestory:
                    HandleForNpcDestory(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcExpression:
                    HandleForNpcExpression(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.NpcEquip:
                    HandleForNpcEquip(itemData);
                    break;


                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Talk:
                    isNext = HandleForTalk(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AutoNext:
                    isNext = HandleForAutoNext(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.PropPosition:
                    HandleForPropPosition(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.WorkerPosition:
                    HandleForWorkerPosition(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.Effect:
                    HandleForEffect(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SetTime:
                    HandleForSetTime(itemData);
                    break;


                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraPosition:
                    HandleForCameraPosition(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.CameraFollowCharacter:
                    HandleForCameraFollowCharacter(itemData);
                    break;


                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioSound:
                    HandleForAudioSound(itemData);
                    break;
                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.AudioMusic:
                    HandleForAudioMusic(itemData);
                    break;

                case StoryInfoDetailsBean.StoryInfoDetailsTypeEnum.SceneInt:
                    HandleForSceneInt(itemData);
                    break;
            }
        }
        if (isNext)
            NextStoryOrder();
    }

    public void HandleForNpcPosition(StoryInfoDetailsBean itemData)
    {
        //Npc站位
        GameObject objNpc = GetNpcByNpcNum(itemData.num);
        BaseNpcAI npcAI;
        if (objNpc == null)
        {
            npcAI = CreateNpc(itemData);
            objNpc = npcAI.gameObject;
        }
        else
        {
            npcAI = objNpc.GetComponent<BaseNpcAI>();
            npcAI.characterMoveCpt.SetDestinationLocal(transform, new Vector3(itemData.position_x, itemData.position_y));
        }
        npcAI.SetCharacterFace(itemData.face);
    }

    public void HandleForNpcDestory(StoryInfoDetailsBean itemData)
    {
        //删除角色
        int[] npcNum = itemData.npc_destroy.SplitForArrayInt(',');
        float destroyTime = itemData.wait_time;
        foreach (int itemNpcNum in npcNum)
        {
            GameObject objNpc = GetNpcByNpcNum(itemNpcNum);
            //延迟删除
            StartCoroutine(CoroutineForDelayDestoryNpc(destroyTime, objNpc));
        }
    }
    public void HandleForNpcExpression(StoryInfoDetailsBean itemData)
    {
        //表情
        GameObject objItem = GetNpcByNpcNum(itemData.num);
        if (objItem != null)
        {
            NpcAIStoryCpt npcAIStory = objItem.GetComponent<NpcAIStoryCpt>();
            if (npcAIStory != null)
                npcAIStory.SetExpression(itemData.expression);
        }
    }

    public void HandleForNpcEquip(StoryInfoDetailsBean itemData)
    {
        //装备
        GameObject objItem = GetNpcByNpcNum(itemData.num);
        if (objItem != null)
        {
            NpcAIStoryCpt npcAIStory = objItem.GetComponent<NpcAIStoryCpt>();
            itemData.GetNpcEquip(npcAIStory.characterData.body.GetSex(), out long hatId, out long clothesId, out long shoesId);
            if (hatId != -1)
                npcAIStory.characterData.equips.hatTFId = hatId;
            if (clothesId != -1)
                npcAIStory.characterData.equips.clothesTFId = clothesId;
            if (shoesId != -1)
                npcAIStory.characterData.equips.shoesTFId = shoesId;
            npcAIStory.SetCharacterData(npcAIStory.characterData);
        }
    }


    public void HandleForSceneInt(StoryInfoDetailsBean itemData)
    {
        try
        {
            //场景物体互动
            GameObject objFind = GameObject.Find(itemData.scene_intobj_name);
            //参数
            List<string> listparameter = itemData.scene_intcomponent_parameters.SplitForListStr(',');
            //通过反射调取方法
            ReflexUtil.GetInvokeMethod(objFind, itemData.scene_intcomponent_name, itemData.scene_intcomponent_method, listparameter);
        }
        catch
        {

        }
    }

    public bool HandleForTalk(StoryInfoDetailsBean itemData)
    {
        //进入对话;
        UIGameText uiComponent = UIHandler.Instance.OpenUIAndCloseOther<UIGameText>();
        uiComponent.SetData(TextEnum.Story, itemData.text_mark_id);
        return false;
    }

    public bool HandleForAutoNext(StoryInfoDetailsBean itemData)
    {
        //剧情自动跳转
        StartCoroutine(CoroutineForAutoNext(itemData.wait_time));
        return false;
    }

    public void HandleForPropPosition(StoryInfoDetailsBean itemData)
    {
        //道具位置
        GameObject objProp = GetPropByNpcNum(itemData.num);
        if (objProp == null)
        {
            CreateProp(itemData);
        }
        else
        {
            CharacterMoveCpt characterMove = objProp.GetComponent<CharacterMoveCpt>();
            //如果可以移动
            if (characterMove)
            {
                characterMove.SetDestinationLocal(transform, new Vector3(itemData.position_x, itemData.position_y));
            }
            //如果不能移动则直接设置坐标
            else
            {
                objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
            }
        }
    }

    public void HandleForCameraPosition(StoryInfoDetailsBean itemData)
    {
        //设置摄像头位置
        BaseControl baseControl = GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
        Vector3 cameraWorldPosition = transform.TransformPoint(new Vector3(itemData.position_x, itemData.position_y));
        baseControl.SetCameraFollowObj(null);
        baseControl.SetFollowPosition(cameraWorldPosition);
    }

    public void HandleForCameraFollowCharacter(StoryInfoDetailsBean itemData)
    {
        //设置摄像头位置
        GameObject objNpc = GetNpcByNpcNum(itemData.num);
        BaseControl baseControl = GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story);
        baseControl.SetCameraFollowObj(objNpc);
    }

    public void HandleForWorkerPosition(StoryInfoDetailsBean itemData)
    {
        CreateAllWorker(itemData);
    }

    public void HandleForEffect(StoryInfoDetailsBean itemData)
    {
        Vector3 effectPosition = new Vector3(itemData.position_x,itemData.position_y);
        string effectName = itemData.key_name;
        float effectTime = itemData.wait_time;

        EffectBean effectData = new EffectBean();
        effectData.effectName = effectName;
        effectData.effectPosition = effectPosition;
        effectData.timeForShow = effectTime;
        EffectHandler.Instance.ShowEffect(effectData);
        //EffectHandler.Instance.PlayEffect(effectName, effectPosition, effectTime);
    }

    public void HandleForSetTime(StoryInfoDetailsBean itemData)
    {
        GameTimeHandler.Instance.SetTime(itemData.time_hour,itemData.time_minute);
    }

    public void HandleForAudioSound(StoryInfoDetailsBean itemData)
    {
        //播放音效
        AudioHandler.Instance.PlaySound(itemData.GetAudioSound());
    }
    public void HandleForAudioMusic(StoryInfoDetailsBean itemData)
    {
        //播放音效
        AudioHandler.Instance.PlayMusicForLoop(itemData.GetAudioMusic());
    }



    /// <summary>
    /// 下一个剧情点
    /// </summary>
    public void NextStoryOrder()
    {
        mStoryOrder++;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(mStoryOrder);
        if (listOrderData.IsNull())
        {
            //清理故事场景
            ClearStoryScene();
            //没有剧情。完结
            GameEventHandler.Instance.SetEventStatus(GameEventHandler.EventStatusEnum.EventEnd);
        }
        else
            CreateStoryScene(listOrderData);
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
                if (itemNpc.name.Equals("character_" + npcNum))
                {
                    return itemNpc;
                }
            }
        }
        return null;
    }

    public GameObject GetPropByNpcNum(int propNum)
    {
        if (listPropObj != null)
        {
            foreach (GameObject itemProp in listPropObj)
            {
                if (itemProp.name.Equals("prop_" + propNum))
                {
                    return itemProp;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 生成NPC
    /// </summary>
    /// <param name="itemData"></param>
    public BaseNpcAI CreateNpc(StoryInfoDetailsBean itemData)
    {
        GameObject objNpcModel = StoryInfoHandler.Instance.manager.objNpcModel;
        GameObject objNpc = Instantiate(transform.gameObject, objNpcModel);
        objNpc.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
        listNpcObj.Add(objNpc);
        NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
        CharacterBean characterData;
        if (itemData.npc_id == 0)
        {
            //使用自己的数据
            GameControlHandler.Instance.manager.GetControl<ControlForStoryCpt>(GameControlHandler.ControlEnum.Story).SetCameraFollowObj(objNpc);
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            characterData = gameData.userCharacter;
        }
        else if (itemData.npc_id == -1)
        {
            // 使用妻子的数据
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            FamilyDataBean familyData =  gameData.GetFamilyData();
            characterData = familyData.mateCharacter as CharacterBean;
        }
        else
            characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(itemData.npc_id);
        //设置编号
        objNpc.name = "character_" + itemData.num;
        CharacterBean copyCharacterData = ClassUtil.DeepCopyByBin<CharacterBean>(characterData);
        aiNpc.SetCharacterData(copyCharacterData);
        //默认设置NPC速度为1
        aiNpc.characterMoveCpt.SetMoveSpeed(1);
        return aiNpc;
    }

    /// <summary>
    /// 生成所有员工
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateAllWorker(StoryInfoDetailsBean itemData)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterBean> listCharacterData = gameData.listWorkerCharacter;
        if (listCharacterData.IsNull())
            return;
        if (itemData.horizontal == 0)
            itemData.horizontal = 1;
        int horizontalNumber = listCharacterData.Count / itemData.horizontal;
        int tempHorizontalNumber = 0;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            CharacterBean characterData = listCharacterData[i];
            GameObject objNpcModel = StoryInfoHandler.Instance.manager.objNpcModel;
            GameObject objNpc = Instantiate(transform.gameObject, objNpcModel);

            int tempHorizontal = i / horizontalNumber;
            if (tempHorizontalNumber >= horizontalNumber)
            {
                tempHorizontalNumber = 0;
            }
            float positionX = itemData.position_x + tempHorizontalNumber * itemData.offset_x;
            float positionY = itemData.position_y + tempHorizontal * itemData.offset_y;

            tempHorizontalNumber++;

            objNpc.transform.localPosition = new Vector3(positionX, positionY);
            NpcAIStoryCpt aiNpc = objNpc.GetComponent<NpcAIStoryCpt>();
            //设置编号
            objNpc.name = "character_" + (i + 1001);
            aiNpc.SetCharacterData(characterData);
            aiNpc.SetCharacterFace(itemData.face);
            listNpcObj.Add(objNpc);
        }
    }

    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="itemData"></param>
    public void CreateProp(StoryInfoDetailsBean itemData)
    {
        GameObject objPropModel = StoryInfoHandler.Instance.manager.GetStoryPropModelByName(itemData.key_name);
        GameObject objProp = Instantiate(transform.gameObject, objPropModel);

        listPropObj.Add(objProp);
        objProp.name = "prop_" + itemData.num;

        //设置位置和朝向
        objProp.transform.localPosition = new Vector3(itemData.position_x, itemData.position_y);
        Vector3 bodyScale = objProp.transform.localScale;
        switch (itemData.face)
        {
            case 1:
                bodyScale.x = -1;
                break;

            case 2:
                bodyScale.x = 1;
                break;
        }
        objProp.transform.localScale = bodyScale;

        //如果有移动控件
        CharacterMoveCpt characterMove = objProp.GetComponent<CharacterMoveCpt>();
        if (characterMove != null)
        {
            characterMove.SetMoveSpeed(1);
        }
    }

    /// <summary>
    /// 清理剧情场景
    /// </summary>
    public void ClearStoryScene()
    {
        mGameCookingData = null;
        CptUtil.RemoveChildsByActive(transform);
        gameObject.transform.position = Vector3.zero;
        listNpcObj.Clear();
        listPropObj.Clear();
        mStoryOrder = 1;
    }

    /// <summary>
    /// 合并烹饪游戏数据
    /// </summary>
    /// <param name="listData"></param>
    /// <param name="gameCookingData"></param>
    private void MergeDataForGameCooking(List<StoryInfoDetailsBean> listData, MiniGameCookingBean gameCookingData)
    {

    }

    #region  获取故事详情回调
    public void SetStoryDetailsData(List<StoryInfoDetailsBean> listData)
    {
        ClearStoryScene();
        listStoryDetails = listData;
        List<StoryInfoDetailsBean> listOrderData = GetStoryDetailsByOrder(mStoryOrder);
        //如果是迷你烹饪游戏的剧情 则需要合并一下数据
        if (mGameCookingData != null)
        {
            MergeDataForGameCooking(listOrderData, mGameCookingData);
        }
        CreateStoryScene(listOrderData);

    }
    #endregion

    /// <summary>
    /// 协程 延迟删除NPC
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="objNpc"></param>
    /// <returns></returns>
    protected IEnumerator CoroutineForDelayDestoryNpc(float waitTime, GameObject objNpc)
    {
        yield return new WaitForSeconds(waitTime);
        if (objNpc != null)
        {
            listNpcObj.Remove(objNpc);
            Destroy(objNpc);
        }

    }

    /// <summary>
    /// 协程-自动开始下一个
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    protected IEnumerator CoroutineForAutoNext(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NextStoryOrder();
    }
}