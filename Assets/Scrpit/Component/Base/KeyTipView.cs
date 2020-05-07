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
        if (GameCommonInfo.GameConfig.statusForKeyTip==0)
        {
            cgKeyTip.alpha = 0;
        }
        else if (GameCommonInfo.GameConfig.statusForKeyTip == 1)
        {
            cgKeyTip.alpha = 1;
        }
    }
}