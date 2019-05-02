using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemGameMenuFoodCpt : BaseMonoBehaviour, IRadioButtonCallBack
{
    public Text tvName;
    public InfoFoodPopupButton pbFood;
    public Image ivFood;
    public GameObject objPriceS;
    public Text tvPriceS;
    public GameObject objPriceM;
    public Text tvPriceM;
    public GameObject objPriceL;
    public Text tvPriceL;
    public RadioButtonView rbShow;
    public Text tvShow;
    public InfoPromptPopupButton pbReputation;
    public Image ivReputation;

    public GameDataManager gameDataManager;

    public Sprite spReputation1;
    public Sprite spReputation2;
    public Sprite spReputation3;

    public InnFoodManager innFoodManager;
    public MenuOwnBean menuOwnData;
    public MenuInfoBean foodData;

    private void Start()
    {
        if (rbShow != null)
            rbShow.SetCallBack(this);
        if (pbReputation != null)
            pbReputation.SetContent("知名度");
    }

    private void Update()
    {
        if (menuOwnData == null|| foodData==null)
            return;
        //知名度设置
        if (menuOwnData.sellNumber < 100)
        {
            pbReputation.gameObject.SetActive(false);
        }
        else
        {
            pbReputation.gameObject.SetActive(true);
            if (menuOwnData.sellNumber >= 100 && menuOwnData.sellNumber < 1000)
            {
                ivReputation.sprite = spReputation1;
            }
            else if (menuOwnData.sellNumber >= 1000 && menuOwnData.sellNumber < 10000)
            {
                ivReputation.sprite = spReputation2;
            }
            else if (menuOwnData.sellNumber >= 10000)
            {
                ivReputation.sprite = spReputation3;
            }
        }
        //设置材料是否足够
        if (gameDataManager.gameData.CheckCookFood(foodData))
        {
            tvName.color =Color.black;
            tvShow.color= Color.black;
        }
        else
        {
            tvName.color = Color.red;
            tvShow.color = Color.red;
        }
    }

    public void SetData(MenuOwnBean menuOwn, MenuInfoBean data)
    {
        foodData = data;
        menuOwnData = menuOwn;
        //设置详细信息弹窗
        if (pbFood != null)
            pbFood.SetData(menuOwnData, foodData);

        Sprite spFood = innFoodManager.GetFoodSpriteByName(foodData.icon_key);
        //食物图标设置
        if (ivFood != null)
        {
            ivFood.sprite = spFood;
        }
        //名字设置
        if (tvName != null)
        {
            tvName.text = data.name;
        }
        //价格设置
        if (data.price_l == 0)
        {
            if (objPriceL != null)
                objPriceL.SetActive(false);
        }
        else
        {
            if (objPriceL != null)
                objPriceL.SetActive(true);
            if (tvPriceL != null)
                tvPriceL.text = data.price_l + "";
        }
        if (data.price_m == 0)
        {
            if (objPriceM != null)
                objPriceM.SetActive(false);
        }
        else
        {
            if (objPriceM != null)
                objPriceM.SetActive(true);
            if (tvPriceM != null)
                tvPriceM.text = data.price_m + "";
        }
        if (data.price_s == 0)
        {
            if (objPriceS != null)
                objPriceS.SetActive(false);
        }
        else
        {
            if (objPriceS != null)
                objPriceS.SetActive(true);
            if (tvPriceS != null)
                tvPriceS.text = data.price_s + "";
        }
        //菜单是否买卖设置
        if (menuOwnData.isSell)
        {
            if (rbShow != null)
                rbShow.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            if (tvShow != null)
                tvShow.text = "售卖中";
        }
        else
        {
            if (rbShow != null)
                rbShow.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
            if (tvShow != null)
                tvShow.text = "隐藏中";
        }

    }

    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStates buttonStates)
    {
        if (view == rbShow && tvShow != null)
        {
            switch (buttonStates)
            {
                case RadioButtonView.RadioButtonStates.Selected:
                    tvShow.text = "售卖中";
                    menuOwnData.isSell = true;
                    break;
                case RadioButtonView.RadioButtonStates.Unselected:
                    tvShow.text = "隐藏中";
                    menuOwnData.isSell = false;
                    break;
            }
        }
    }
}