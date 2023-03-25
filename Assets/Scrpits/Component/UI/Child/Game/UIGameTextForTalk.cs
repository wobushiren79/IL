using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameTextForTalk : BaseUIView
{
    //文本容器
    public RectTransform rtfTextContent;
    public ScrollRect scrollRect;
    //内容
    public Text tvContent;
    public Text tvName;
    public Image ivFavorability;
    public GameObject objNext;
    public CharacterUICpt characterUICpt;

    public TextInfoBean textData;
    public Tweener tweenerText;

    public GameObject objSelectContent;
    public GameObject objSelectModel;

    public Text tvMoneyL;
    public Text tvMoneyM;
    public Text tvMoneyS;

    public override void CloseUI()
    {
        base.CloseUI();
        tweenerText = null;
    }

    private void Update()
    {
        if (textData == null)
        {
            return;
        }
        if (Input.GetButtonDown(InputInfo.Interactive_E)||Input.GetButtonDown(InputInfo.Confirm))
        {
            if (tweenerText != null && tweenerText.IsActive() && tweenerText.IsPlaying())
            {
                tweenerText.Complete();
                //刷新控件
                if (rtfTextContent != null)
                    GameUtil.RefreshRectTransform(rtfTextContent);
            }
            else
            {
                //当是选择对话 不能跳过
                if (textData.GetTextType() ==  TextInfoTypeEnum.Select|| textData.GetTextType() == TextInfoTypeEnum.Talk )
                {

                }
                else
                {
                    UIGameText uIGameText = UIHandler.Instance.GetUI<UIGameText>();
                    uIGameText.NextText();
                }
            }
        }
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (tvMoneyL != null)
        {
            tvMoneyL.text = gameData.moneyL + "";
        }
        if (tvMoneyM != null)
        {
            tvMoneyM.text = gameData.moneyM + "";
        }
        if (tvMoneyS != null)
        {
            tvMoneyS.text = gameData.moneyS + "";
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="textData"></param>
    /// <param name="listTextInfo"></param>
    public void SetData(TextInfoBean textData, List<TextInfoBean> listTextInfo)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //清空选项
        CptUtil.RemoveChildsByName(objSelectContent.transform, "SelectButton", true);
        //清空文本
        tvContent.text = "";
        tvName.text = "";
        //回到顶部
        scrollRect.verticalNormalizedPosition = 0;
        this.textData = textData;
        //选择对话 特殊处理 增加选择框
        if (textData.GetTextType() == TextInfoTypeEnum.Select
            || textData.GetTextType() == TextInfoTypeEnum.Talk)
        {
            objNext.gameObject.SetActive(false);
            foreach (TextInfoBean itemData in listTextInfo)
            {
                //提示文本
                if (itemData.select_type == 0)
                {
                    this.textData = itemData;
                }
                // 选项
                else
                {
                    GameObject objSelect = Instantiate(objSelectModel, objSelectContent.transform);
                    objSelect.SetActive(true);
                    ItemGameTextSelectCpt itemCpt = objSelect.GetComponent<ItemGameTextSelectCpt>();
                    itemCpt.SetData(itemData);
                }
            }
        }
        else
        {
            objNext.gameObject.SetActive(true);
            //添加奖励
            AddReward(textData.reward_data);
        }
        //正常文本处理
      
        //查询角色数据
        CharacterBean characterData;
        if (textData.user_id == 0)
            characterData = gameData.userCharacter;
        else
            characterData = NpcInfoHandler.Instance.manager.GetCharacterDataById(textData.user_id);
        if (characterData == null)
        {
            LogUtil.LogError("文本展示没有找到该文本发起者");
            return;
        }
        //名字设置
        SetName(characterData.baseInfo.titleName, characterData.baseInfo.name, textData.name);
        //设置角色形象
        SetCharacterUI(characterData);
        //设置正文内容
        SetContent(textData.content);
        //添加好感度
        AddFavorability(textData.user_id, textData.add_favorability);
        //场景人物表情展示
        ShowSceneExpression(textData.scene_expression);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="titleName">称号</param>
    /// <param name="name">名字</param>
    /// <param name="remarkName">备用名字</param>
    public void SetName(string titleName, string name,string remarkName)
    {
        if (tvName != null)
        {
            if (remarkName.IsNull())
            {
                string totalName = "";
                if (titleName.IsNull()|| name.IsNull())
                {
                    if (!titleName.IsNull())
                    {
                        totalName += titleName;
                    }
                    if (!name.IsNull())
                    {
                        totalName += name;
                    }
                }
                else
                {
                    totalName = titleName + "-" + name;
                }

                tvName.text = totalName;
            }
            else
            {
                tvName.text = remarkName;
            }
        }
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterUI(CharacterBean characterData)
    {
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置正文内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
        {
            tvContent.text = "";
            UIGameText uIGameText = UIHandler.Instance.GetUI<UIGameText>();
            string contentDetails = uIGameText.SetContentDetails(content);
            //如果时停了就不播放动画了
            if (Time.timeScale == 0)
            {
                tvContent.text = contentDetails;
                //刷新控件大小
                if (rtfTextContent != null)
                    GameUtil.RefreshRectTransform(rtfTextContent);
            }
            else
            {
                tweenerText = tvContent.DOText(contentDetails, textData.content.Length / 8f).OnComplete(delegate {
                    //刷新控件大小
                    if (rtfTextContent != null)
                        GameUtil.RefreshRectTransform(rtfTextContent);
                });
            } 
        }
    }

    /// <summary>
    /// 增加好感
    /// </summary>
    /// <param name="characterId"></param>
    /// <param name="favorablility"></param>
    public void AddFavorability(long characterId, int favorablility)
    {
        if (favorablility != 0)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            CharacterFavorabilityBean favorabilityData = gameData.GetCharacterFavorability(characterId);
            favorabilityData.AddFavorability(favorablility);
            //好感动画
            if (ivFavorability != null&& favorablility>0)
            {
                ivFavorability.transform.localScale = new Vector3(1, 1, 1);
                ivFavorability.transform.DOComplete();
                ivFavorability.gameObject.SetActive(true);
                ivFavorability.transform.DOScale(new Vector3(0, 0, 0), 1).From().SetEase(Ease.OutBack).OnComplete(delegate ()
                {
                    ivFavorability.gameObject.SetActive(false);
                });
                ivFavorability.DOColor(new Color(1, 1, 1, 0), 1).From();
            }
            //回调
            UIGameText uIGameText = UIHandler.Instance.GetUI<UIGameText>();
            if (uIGameText.callBack != null)
                uIGameText.callBack.UITextAddFavorability(characterId, favorablility);
        }
    }

    /// <summary>
    /// 增加奖励
    /// </summary>
    /// <param name="reward"></param>
    public void AddReward(string reward)
    {
        if (reward.IsNull())
            return;
        RewardTypeEnumTools.CompleteReward(null,reward);
    }

    /// <summary>
    /// 展示场景表情显示
    /// </summary>
    /// <param name="sceneExpressionData"></param>
    public void ShowSceneExpression(string sceneExpressionData)
    {
        if (!sceneExpressionData.IsNull())
        {
            UIGameText uIGameText = UIHandler.Instance.GetUI<UIGameText>();
            if (uIGameText.callBack != null)
            {
                Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum> mapData = new Dictionary<int, CharacterExpressionCpt.CharacterExpressionEnum>();
                List<string> listData = sceneExpressionData.SplitForListStr(',');
                for (int i = 0; i < listData.Count; i += 2)
                {
                    int mapKey = int.Parse(listData[i]);
                    CharacterExpressionCpt.CharacterExpressionEnum mapValue = (CharacterExpressionCpt.CharacterExpressionEnum)int.Parse(listData[i + 1]);
                    mapData.Add(mapKey, mapValue);
                }
                uIGameText.callBack.UITextSceneExpression(mapData);
            }
        }
    }

}