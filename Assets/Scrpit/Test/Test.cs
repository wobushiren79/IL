using UnityEngine;
using UnityEngine.UI;



using Pathfinding;
using System;
using System.Threading.Tasks;

public class Test : BaseMonoBehaviour
{
    private void Start()
    {
        MiniGameBirthHandler miniGameBirthHandler = MiniGameHandler.Instance.handlerForBirth;
        MiniGameBirthBean miniGameBirth = new MiniGameBirthBean();
        miniGameBirthHandler.InitGame(miniGameBirth);
    }
}
