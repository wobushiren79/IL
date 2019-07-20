using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameGuildAchievementCpt : BaseMonoBehaviour
{
    public Image ivIcon;
    public Image ivBackground;

    public GameItemsManager gameItemsManager;
    public GameDataManager gameDataManager;

    public Sprite spIconUnknow;
    public Sprite spBackPass;
    public Sprite spBackUnLock;
    public Sprite spBackLock;
    public Material materialGray;

    public AchievementInfoBean achievementInfo;

    public void SetData(AchievementInfoBean data)
    {
        this.achievementInfo = data;
        SetIcon(data.id, data.pre_ach_id, data.icon_key);
    }

    public void SetIcon(long achId, long preId, string iconKey)
    {
        if (gameItemsManager == null || ivIcon == null || gameDataManager == null || ivBackground == null)
            return;
        //检测是否拥有该成就
        bool hasAch = gameDataManager.gameData.GetAchievementData().CheckAchievementList(achId);
        if (hasAch)
        {
            SetAchStatus(1);
            return;
        }
        //检测前置成就
        if (preId == 0)
        {
            //检测是否符合条件
            if (CheckAchieve())
            {
                SetAchStatus(3);
            }
            else
            {
                SetAchStatus(2);
            }
        }
        else
        {
            bool hasPre = gameDataManager.gameData.GetAchievementData().CheckAchievementList(preId);
            if (hasPre)
            {
                //检测是否符合条件
                if (CheckAchieve())
                {
                    SetAchStatus(3);
                }
                else
                {
                    SetAchStatus(2);
                }
            }
            else
            {
                SetAchStatus(0);
            }
        }
    }

    public void SetAchStatus(int status)
    {
        if (achievementInfo == null || ivIcon == null || ivBackground == null)
            return;
        switch (status)
        {
            case 0:
                //未知
                ivIcon.sprite = spIconUnknow;
                ivBackground.sprite = spBackLock;
                break;
            case 1:
                //已解锁
                SetIcon(achievementInfo.icon_key, null);
                ivBackground.sprite = spBackUnLock;
                break;
            case 2:
                //未解锁 不满足条件
                SetIcon(achievementInfo.icon_key, materialGray);
                ivBackground.sprite = spBackLock;
                break;
            case 3:
                //未解锁 满足条件  
                SetIcon(achievementInfo.icon_key, materialGray);
                ivBackground.sprite = spBackPass;
                break;
        }
    }

    public void SetIcon(string iconKey, Material material)
    {
        if (gameItemsManager == null || ivIcon == null)
            return;
        Sprite spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
        if (spIcon != null)
            ivIcon.sprite = spIcon;
        else
            ivIcon.sprite = spIconUnknow;
        ivIcon.material = material;
    }

    public bool CheckAchieve()
    {
        if (achievementInfo == null || gameDataManager == null)
            return false;
        return achievementInfo.CheckAchievement(gameDataManager.gameData);
    }
}