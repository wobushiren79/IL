using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class CharacterMoveCpt : BaseMonoBehaviour
{
    //移动差值
    public float lerpOffset = 0.9f;
    //移动速度
    public float moveSpeed = 1;

    //角色动画
    public Animator characterAnimtor;
    public NavMeshAgent navMeshAgent;
    public GameObject characterBodyObj;
    public Rigidbody2D characterRigidbody;

    //是否手动移动
    public bool isManualMove = false;

    public float minMoveX = 0;
    public float maxMoveX = 0;
    public float minMoveY = 0;
    public float maxMoveY = 0;

    private void Awake()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }
    }

    private void Start()
    {
        lastPosition = transform.position;
    }

    private Vector3 lastPosition;

    private void FixedUpdate()
    {
        if (!isManualMove && navMeshAgent != null)
        {
            if (navMeshAgent.hasPath)
            {
                if (characterAnimtor != null)
                {
                    characterAnimtor.SetInteger("State", 1);
                }
                //Move(navMeshAgent.nextPosition);
                //转向
                Vector3 theScale = characterBodyObj.transform.localScale;
                if (characterBodyObj.transform.position.x - lastPosition.x < 0)
                {
                    theScale.x = -Mathf.Abs(theScale.x);
                }
                else if (characterBodyObj.transform.position.x - lastPosition.x > 0)
                {
                    theScale.x = Mathf.Abs(theScale.x);
                }
                characterBodyObj.transform.localScale = theScale;
            }
            else
            {
                //是否在计算路径
                if (!navMeshAgent.pathPending)
                    StopAnim();
            }
            lastPosition = transform.position;
        }
    }


    /// <summary>
    /// 自动移动
    /// </summary>
    /// <param name="position">目的地</param>
    public bool SetDestination(Vector3 position)
    {
        isManualMove = false;
        bool canGo = true;
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updatePosition = true;
            // NavMesh.avoidancePredictionTime = 5f;
            navMeshAgent.speed = moveSpeed;
            canGo = navMeshAgent.SetDestination(position);
        }
        return canGo;
    }

    /// <summary>
    /// 本地移动
    /// </summary>
    /// <param name="tfPar"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool SetDestinationLocal(Transform tfPar, Vector3 position)
    {
       Vector3 worldPos= tfPar.TransformPoint(position);
       return SetDestination(worldPos);
    }

    /// <summary>
    /// 获取路径状态
    /// </summary>
    /// <returns></returns>
    public NavMeshPathStatus GetAutoMovePathStatus()
    {
        return navMeshAgent.pathStatus;
    }

    /// <summary>
    /// 控制移动
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Move(float x, float y)
    {
        isManualMove = true;
        //停止自动寻路
        StopAutoMove();
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 1);
        }
        //转向
        Vector3 theScale = characterBodyObj.transform.localScale;
        if (x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        characterBodyObj.transform.localScale = theScale;
        //Vector2 lerpPosition = Vector3.Lerp(Vector2.zero, new Vector2(x, y), lerpOffset);
        // transform.Translate(movePosition * moveSpeed * Time.deltaTime);
        Vector3 movePosition = transform.position + new Vector3(x, y) * moveSpeed * Time.deltaTime;
        characterRigidbody.MovePosition(movePosition);
        BoundaryMove();

    }

    /// <summary>
    /// 自动寻路
    /// </summary>
    /// <param name="movePosition"></param>
    public void Move(Vector3 movePosition)
    {
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 1);
        }
        //转向
        Vector3 theScale = characterBodyObj.transform.localScale;
        if (movePosition.x - characterBodyObj.transform.position.x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (movePosition.x - characterBodyObj.transform.position.x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        characterBodyObj.transform.localScale = theScale;
        //transform.position = movePosition;
        BoundaryMove();
    }

    /// <summary>
    ///  边界处理
    /// </summary>
    public void BoundaryMove()
    {
        Vector3 newPosition = transform.position;
        if (maxMoveX != 0 && transform.position.x > maxMoveX)
        {
            newPosition.x = maxMoveX;
        }
        if (minMoveX != 0 && transform.position.x < minMoveX)
        {
            newPosition.x = minMoveX;
        }
        if (maxMoveY != 0 && transform.position.y > maxMoveY)
        {
            newPosition.y = maxMoveY;
        }
        if (minMoveY != 0 && transform.position.y < minMoveY)
        {
            newPosition.y = minMoveY;
        }
        transform.position = newPosition;
    }


    public void StopAnim()
    {
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 0);
        }
    }

    /// <summary>
    /// 停止自动寻路
    /// </summary>
    public void StopAutoMove()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }
    }

    /// <summary>
    /// 自动寻路是否停止
    /// </summary>
    /// <returns></returns>
    public bool IsAutoMoveStop()
    {
        if (navMeshAgent != null && navMeshAgent.pathPending)
        {
            return false;
        }
        if (navMeshAgent != null && navMeshAgent.hasPath)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}