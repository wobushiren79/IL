using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemTownBeautySalonCpt : ItemGameBaseCpt
{
    public GameObject objLock;
    public GameObject objUnlock;

    public Image ivIcon;
    public Button btSubmit;
    public PriceShowView priceShow;

    public long priceL;
    public long priceM;
    public long priceS;
    public BodyTypeEnum type;
    public string data;

    protected UIGameManager uiGameManager;
    protected ICallBack callBack;
    private void Awake()
    {
        uiGameManager = GetUIManager<UIGameManager>();
        if (btSubmit)
            btSubmit.onClick.AddListener(OnClickForSubmit);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    public void SetData(BodyTypeEnum type, string data, bool isLock)
    {
        this.type = type;
        this.data = data;
        if (isLock)
        {
            objLock.SetActive(true);
            objUnlock.SetActive(false);
        }
        else
        {
            objLock.SetActive(false);
            objUnlock.SetActive(true);
        }
        Sprite spIcon = null;
        priceL = 0;
        priceM = 0;
        priceS = 0;

        switch (type)
        {
            case BodyTypeEnum.Hair:
                priceM = 1;
                spIcon = CharacterBodyHandler.Instance.manager.GetHairSpriteByName(data);
                break;
            case BodyTypeEnum.Eye:
                priceL = 10;
                spIcon = CharacterBodyHandler.Instance.manager.GetEyeSpriteByName(data);
                break;
            case BodyTypeEnum.Mouth:
                priceM = 10;
                spIcon = CharacterBodyHandler.Instance.manager.GetMouthSpriteByName(data);
                break;
            case BodyTypeEnum.Skin:
                priceL = 100;
                if (data.Equals("Def"))
                {
                    spIcon = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName("character_body_man");
                }
                else
                {
                    spIcon = CharacterBodyHandler.Instance.manager.GetTrunkSpriteByName(data+"_1");
                }
                break;
        }

        SetIcon(spIcon);
        SetPrice(priceL, priceM, priceS);
    }

    public void SetIcon(Sprite spIcon)
    {
        ivIcon.sprite = spIcon;
    }

    public void SetPrice(long priceL, long priceM, long priceS)
    {
        priceShow.SetPrice(1, priceL, priceM, priceS, 0, 0, 0, 0, 0);
    }

    public void OnClickForSubmit()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (callBack != null)
            callBack.SelectItem(type,data,priceL,priceM,priceS);
    }

    public interface ICallBack
    {
         void SelectItem(BodyTypeEnum type,string data,long priceL,long priceM,long priceS);
    }
}