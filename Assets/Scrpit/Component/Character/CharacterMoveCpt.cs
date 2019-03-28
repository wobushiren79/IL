using UnityEngine;
using UnityEditor;

public class CharacterMoveCpt : BaseMonoBehaviour
{
    //移动差值
    public float lerpOffset = 0.9f;
    //移动速度
    public float moveSpeed = 1;

    //角色动画
    public Animator characterAnimtor;

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Move(float x, float y)
    {
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 1);
        }
        //转向
        Vector3 theScale = transform.localScale;
        if (x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        transform.localScale = theScale;
        //移动
        Vector3 movePosition = Vector3.Lerp(Vector3.zero, new Vector3(x, y), lerpOffset);
        transform.Translate(movePosition * moveSpeed * Time.deltaTime);
    }

    public void Stop()
    {
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 0);
        }
    }
}