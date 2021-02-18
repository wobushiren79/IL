using UnityEngine;
using UnityEditor;

public class KeyTipView : BaseMonoBehaviour
{
    protected CanvasGroup cgKeyTip;

    private void Awake()
    {
        cgKeyTip = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForKeyTip == 0)
        {
            cgKeyTip.alpha = 0;
        }
        else if (gameConfig.statusForKeyTip == 1)
        {
            cgKeyTip.alpha = 1;
        }
    }
}