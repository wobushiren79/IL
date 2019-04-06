using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    public CharacterMoveCpt characterMoveCpt;
    public Transform targetTF;
    private void Start()
    {
        characterMoveCpt.SetDestination(targetTF.position);
    }

    private void Update()
    {

    }
}
