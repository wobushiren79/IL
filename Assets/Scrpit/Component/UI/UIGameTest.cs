using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameTest : BaseUIComponent
{
    public InputField etStoryId;
    public Button btStoryCreate;

    private void Start()
    {
        if (btStoryCreate != null)
            btStoryCreate.onClick.AddListener(CreateStory);
    }

    public void CreateStory()
    {
        if (long.TryParse(etStoryId.text,out long storyId))
        {
            EventHandler.Instance.EventTriggerForStory(storyId);
        } 
    }
}