using UnityEngine;
using System.Collections;

public class SignForInfiniteTowersCpt : MonoBehaviour
{

    public TextMesh tvContent;

    public void SetData(string content)
    {
        if (tvContent)
            tvContent.text = content;
    }


}