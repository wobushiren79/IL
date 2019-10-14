using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemMiniGameEndResultLoseCpt : BaseMonoBehaviour
{
    public Text tvContent;


    public void SetContent(string content)
    {
        if (tvContent != null)
           tvContent.text = content.Replace("\\n","\n");
    }
}