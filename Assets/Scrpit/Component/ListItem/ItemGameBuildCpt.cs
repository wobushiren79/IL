using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGameBuildCpt : BaseMonoBehaviour
{
    public ItemBean itemData;
    public BuildItemBean buildData;

    public Button btBuild;

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
    }

    public void StartBuild()
    {

    }

}