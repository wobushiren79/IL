using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

using Pathfinding;
public class Test : BaseMonoBehaviour
{
    public Transform target;

    public Transform otherPostion;
    public Transform otherTarget;

    public AIPath aiPath;
    public AIDestinationSetter destinationSetter;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position)<=0.1f)
        {
            destinationSetter.target = otherTarget;
            transform.position = otherPostion.position;
        }
    }

}
