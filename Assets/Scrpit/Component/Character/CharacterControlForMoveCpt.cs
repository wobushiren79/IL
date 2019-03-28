using UnityEngine;
using UnityEditor;

public class CharacterControlForMoveCpt : BaseMonoBehaviour
{
    //角色移动组建
    public CharacterMoveCpt characterMoveCpt;

    private void FixedUpdate()
    {
        if (characterMoveCpt == null)
            return;
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        if(hMove==0&& vMove == 0)
        {
            characterMoveCpt.Stop();
        }
        else
        {
            characterMoveCpt.Move(hMove, vMove);
        }
      
    }
}