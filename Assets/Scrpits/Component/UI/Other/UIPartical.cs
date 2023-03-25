using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIPartical : MonoBehaviour
{
    public RectTransform rtfTarget;

    private void OnGUI()
    {
        float x = rtfTarget.localScale.x;
        float y = rtfTarget.localScale.y;
        transform.localScale = new Vector3(x, y);
    }

}
