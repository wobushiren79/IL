using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    public NavMeshAgent agent;
    public Transform targetTF;

    private void Start()
    {
        agent.SetDestination(targetTF.position);
    }

    private void Update()
    {

    }
}
