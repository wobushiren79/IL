using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    public NavMeshAgent navMeshAgent;
    public Transform targetTF;
    private void Start()
    {
        navMeshAgent.SetDestination(targetTF.position);
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;       
    }
}
