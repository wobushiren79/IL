using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PickForCharacterDialogView : DialogView, ItemGameDialogPickCharacterCpt.ICallBack
{
    public Text tvNull;

    public Button btSortDef;
    public Button btSortLife;
    public Button btSortCook;
    public Button btSortSpeed;
    public Button btSortAccontant;
    public Button btSortCharm;
    public Button btSortForce;
    public Button btSortLucky;


    public GameObject objPickCharacterContainer;
    public GameObject objPickCharacterModel;

    public int pickCharacterMax = 0;
    //是否能不选择角色
    public bool isNullSelect = false;
    public List<CharacterBean> listCharacterData = new List<CharacterBean>();

    public List<CharacterBean> listPickCharacter = new List<CharacterBean>();
    public List<string> listExpelCharacterId = new List<string>();


    public override void Awake()
    {
        base.Awake();

        if (btSortDef)
            btSortDef.onClick.AddListener(OnClickForDef);
        if (btSortLife)
            btSortLife.onClick.AddListener(OnClickForSortLife);
        if (btSortCook)
            btSortCook.onClick.AddListener(OnClickForSortCook);
        if (btSortSpeed)
            btSortSpeed.onClick.AddListener(OnClickForSortSpeed);
        if (btSortAccontant)
            btSortAccontant.onClick.AddListener(OnClickForSortAccontant);
        if (btSortCharm)
            btSortCharm.onClick.AddListener(OnClickForSortCharm);
        if (btSortForce)
            btSortForce.onClick.AddListener(OnClickForSortForce);
        if (btSortLucky)
            btSortLucky.onClick.AddListener(OnClickForSortLucky);
    }

    /// <summary>
    /// 设置选择角色上限
    /// </summary>
    /// <param name="pickCharacterMax"></param>
    public void SetPickCharacterMax(int pickCharacterMax)
    {
        this.pickCharacterMax = pickCharacterMax;
    }
    
    /// <summary>
    /// 设置是否能不选择角色
    /// </summary>
    /// <param name="isNullSelect"></param>
    public void SetIsNullSelect(bool isNullSelect)
    {
        this.isNullSelect = isNullSelect;
    }

    /// <summary>
    /// 设置排出角色
    /// </summary>
    /// <param name="characterId"></param>
    public void SetExpelCharacter(List<string> listCharacterId)
    {
        this.listExpelCharacterId = listCharacterId;
    }

    public void SetExpelCharacter(string characterId)
    {
        SetExpelCharacter(new List<string>() { characterId });
    }

    public override void InitData()
    {
        base.InitData();
        OnClickForDef();
    }

    public void CreateListData()
    {
        listPickCharacter.Clear();
        CptUtil.RemoveChildsByActive(objPickCharacterContainer);
        bool isNull = true;
        foreach (CharacterBean characterData in listCharacterData)
        {
            //筛选不能选择的角色
            bool isExpel = false;
            if (listExpelCharacterId != null)
                foreach (string itemId in listExpelCharacterId)
                {
                    if (itemId.Equals(characterData.baseInfo.characterId))
                    {
                        isExpel = true;
                        break;
                    }
                }
            //排出研究中的角色
            if (characterData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Research)
            {
                isExpel = true;
            }
            if (!isExpel)
            {
                CreatePickItem(characterData);
                isNull = false;
            }
        }
        if (isNull)
        {
            tvNull.gameObject.SetActive(true);
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        SetTitle();
    }

    public override void SubmitOnClick()
    {
        if (listPickCharacter.IsNull() && !isNullSelect)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1032));
        }
        else
        {
            base.SubmitOnClick();
        }
    }

    private void CreatePickItem(CharacterBean characterData)
    {
        GameObject objPick = Instantiate(objPickCharacterContainer, objPickCharacterModel);
        ItemGameDialogPickCharacterCpt pickCpt = objPick.GetComponent<ItemGameDialogPickCharacterCpt>();
        pickCpt.SetData(characterData);
        pickCpt.SetCallBack(this);
    }

    /// <summary>
    /// 获取选取的角色
    /// </summary>
    /// <returns></returns>
    public List<CharacterBean> GetPickCharacter()
    {
        return listPickCharacter;
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    public void SetTitle()
    {
        if (ui_Title != null)
        {
            if(dialogData.title.IsNull())
            {
                ui_Title.text = TextHandler.Instance.manager.GetTextById(4015) + "(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
            }
            else
            {
                ui_Title.text = dialogData.title + "(" + listPickCharacter.Count + "/" + pickCharacterMax + ")";
            }
        }
    }



    public void OnClickForDef()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listCharacterData.Clear();
        listCharacterData.AddRange(gameData.GetAllCharacterData());
        CreateListData();
    }


    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLife()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes(out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.life;
            }).ToList();
        CreateListData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortCook()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.cook;
            }).ToList();
        CreateListData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortSpeed()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.speed;
            }).ToList();
        CreateListData();
    }


    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortAccontant()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.account;
            }).ToList();
        CreateListData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortCharm()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.charm;
            }).ToList();
        CreateListData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortForce()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.force;
            }).ToList();
        CreateListData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLucky()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                data.GetAttributes( out CharacterAttributesBean characterAttributesData);
                return characterAttributesData.lucky;
            }).ToList();
        CreateListData();
    }

    #region 选择回调
    public void PickCharacter(ItemGameDialogPickCharacterCpt itemView, bool isPick, CharacterBean characterData)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (isPick)
        {
            if (listPickCharacter.Count >= pickCharacterMax)
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1052));
                itemView.ChangeStatus();
            }
            else
            {
                listPickCharacter.Add(characterData);
            }
        }
        else
        {
            listPickCharacter.Remove(characterData);
        }
        RefreshUI();
    }
    #endregion
}