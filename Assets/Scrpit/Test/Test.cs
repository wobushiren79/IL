using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Test : MonoBehaviour {

    public NpcAIMiniGameCookingCpt npcCpt;

    private void Start()
    {

 
    }

    private void Update()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            npcCpt.ShowScore(86);
        }
    }


}
