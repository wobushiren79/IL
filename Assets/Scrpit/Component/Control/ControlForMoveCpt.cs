using UnityEngine;
using UnityEditor;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;

    private void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if (hMove == 0 && vMove == 0)
        {

            characterMoveCpt.StopAnim();
        }
        else
        {
            characterMoveCpt.Move(hMove, vMove);
        }   
    }
    public override void StartControl()
    {
        base.StartControl();
        transform.position = new Vector3(0,0,0);
    }
}