using UnityEngine;
using UnityEditor;
using Cinemachine;

public class ControlForMoveCpt : BaseControl
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;

    public CharacterShoutCpt characterShoutCpt;
    private void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if (hMove == 0 && vMove == 0)
        {

            characterMoveCpt.Stop();
        }
        else
        {
            characterMoveCpt.Move(hMove, vMove);
        }
        if (Input.GetMouseButtonDown(0))
        {
            characterShoutCpt.Shout("磨破豆腐");
        }
      
    }
}