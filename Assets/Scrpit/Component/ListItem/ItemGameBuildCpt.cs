using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using static ControlHandler;

public class ItemGameBuildCpt : ItemGameBaseCpt
{
    [Header("控件")]
    public Button btBuild;
    public Image ivIcon;
    public Text tvName;
    public Text tvNumber;

    [Header("数据")]
    public ItemBean itemData;
    public BuildItemBean buildData;

    private void Awake()
    {
        btBuild = GetComponent<Button>();
    }

    private void Start()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(StartBuild);
    }

    public void  SetData(ItemBean itemData, BuildItemBean buildData)
    {
        this.itemData = itemData;
        this.buildData = buildData;

   
        Sprite spFurniture = GetUIManager<UIGameManager>().innBuildManager.GetFurnitureSpriteByName(buildData.icon_key);
        ivIcon.sprite = spFurniture;
        tvNumber.text = "x " + itemData.itemNumber;
        tvName.text = buildData.name;
    }

    public void StartBuild()
    {
        ControlForBuildCpt controlForBuild = (ControlForBuildCpt)GetUIManager<UIGameManager>().controlHandler.GetControl(ControlEnum.Build);
        controlForBuild.SetBuildItem(buildData.id);
    }

}