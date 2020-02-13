using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameTest : BaseUIComponent
{
    public InputField etStoryId;
    public Button btStoryCreate;

    public InputField etItemId;
    public InputField etItemNumber;
    public Button btItemCreate;

    public InputField etBuildItemId;
    public InputField etBuildItemNumber;
    public Button btBuildItemCreate;

    //团队顾客生成
    public InputField etNpcGuestTeamId;
    public Button btNpcGuestTeam;
    public Button btNpcFriend;

    protected NpcEventBuilder npcEventBuilder;

    public override void Awake()
    {
        base.Awake();
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    private void Start()
    {
        if (btStoryCreate != null)
            btStoryCreate.onClick.AddListener(CreateStory);
        if (btItemCreate != null)
            btItemCreate.onClick.AddListener(AddItem);
        if (btBuildItemCreate != null)
            btBuildItemCreate.onClick.AddListener(AddBuildItem);
        if (btNpcGuestTeam != null)
            btNpcGuestTeam.onClick.AddListener(CreateGuestTeam);
        if (btNpcFriend != null)
            btNpcFriend.onClick.AddListener(CreateFriend);
    }

    /// <summary>
    /// 创建故事
    /// </summary>
    public void CreateStory()
    {
        if (long.TryParse(etStoryId.text,out long storyId))
        {
            EventHandler eventHandler= GetUIManager<UIGameManager>().eventHandler;
            eventHandler.EventTriggerForStory(storyId);
        } 
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddItem()
    {
        if (long.TryParse(etItemId.text, out long itemId))
        {

            if (long.TryParse(etItemNumber.text, out long itemNumber))
            {
                UIGameManager uiGameManager= GetUIManager<UIGameManager>();
                uiGameManager.gameDataManager.gameData.AddNewItems(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("道具数量输入错误");
            } 
        }
        else
        {
            LogUtil.LogError("道具ID输入错误");
        }
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    public void AddBuildItem()
    {
        if (long.TryParse(etBuildItemId.text, out long itemId))
        {

            if (long.TryParse(etBuildItemNumber.text, out long itemNumber))
            {
                UIGameManager uiGameManager = GetUIManager<UIGameManager>();
                uiGameManager.gameDataManager.gameData.AddBuildNumber(itemId, itemNumber);
            }
            else
            {
                LogUtil.LogError("建筑数量输入错误");
            }
        }
        else
        {
            LogUtil.LogError("建筑ID输入错误");
        }
    }

    /// <summary>
    /// 生成指定团队
    /// </summary>
    public void CreateGuestTeam()
    {
        if (npcEventBuilder == null)
            return;
        npcEventBuilder.TeamEvent(long.Parse(etNpcGuestTeamId.text));
    }

    /// <summary>
    /// 生成好友
    /// </summary>
    public void CreateFriend()
    {
        if (npcEventBuilder == null)
            return;
        npcEventBuilder.FriendsEvent(long.Parse(etNpcGuestTeamId.text));
    }
}