using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MiniGameAccountEjectorCpt : MonoBehaviour
{
    public SpriteRenderer srHook;
    public SpriteRenderer srHookPlatform;
    public SpriteRenderer srRope;

    void Start()
    {
        
    }
    
    void Update()
    {
        if(srHook&& srHookPlatform&& srRope)
        {
            srRope.transform.position = (srHook.transform.position + srHookPlatform.transform.position) / 2f;
            float angle =  VectorUtil.GetAngle(srHook.transform.position, srHookPlatform.transform.position);
            float localScale = Vector3.Distance(srHook.transform.position , srHookPlatform.transform.position);
            srRope.transform.localEulerAngles = new Vector3(0,0, angle+90);
            srRope.size = new Vector2(0.25f, localScale * 4);
        }
    }
}
