using UnityEngine;
using UnityEditor;
using System;

public class CharacterLifeCpt : BaseMonoBehaviour
{
    //文本 生命条
    public TextMesh tvLife;
    //图片 生命体
    public SpriteRenderer ivLife;

    public void SetData(int currentLife, int maxLife)
    {
        tvLife.text = currentLife + "/" + maxLife;
        if (maxLife == 0)
        {
            maxLife =1;
        }
        ivLife.transform.localScale = new Vector3((float)currentLife / (float)maxLife, 1, 1);
    }
}