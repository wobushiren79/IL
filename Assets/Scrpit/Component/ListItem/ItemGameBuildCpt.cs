using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameBuildCpt : BaseMonoBehaviour
{

    public Button btBuild;
    public Image ivIcon;
    public Text tvName;
    public Text tvNumber;

    public ItemBean itemData;
    public BuildItemBean buildData;

    //数据管理
    public InnBuildManager innBuildManager;
    public ControlForBuildCpt controlForBuild;

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

        Sprite spFurniture=  innBuildManager.GetFurnitureSpriteByName(buildData.icon_key);
        ivIcon.sprite = spFurniture;
        tvNumber.text = "x " + itemData.itemNumber;
        tvName.text = buildData.name;
    }

    public void StartBuild()
    {
        controlForBuild.SetBuildItem(buildData.id);
    }

}