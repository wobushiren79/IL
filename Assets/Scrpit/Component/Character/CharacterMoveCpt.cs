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

    //移动对象
    public GameObject objMove;
    public GameObject objCharacterBody;
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
        if (objMove == null)
            objMove = gameObject;
        mLastPosition = objMove.transform.position;
    }

    private Vector3 mLastPosition;
    //角色动画状态
    private int mAnimState = 0;
   


    private void FixedUpdate()
    {
        if (!isManualMove && navMeshAgent != null)
        {
            if (navMeshAgent.isActiveAndEnabled && Mathf.Abs(navMeshAgent.remainingDistance) > 0.1f)
            {
                SetAnimStatus(1);
                //Move(navMeshAgent.nextPosition);
                //转向
                if (objCharacterBody != null)
                {
                    Vector3 theScale = objCharacterBody.transform.localScale;
                    float offsetMove = objCharacterBody.transform.position.x - mLastPosition.x;
                    if (Mathf.Abs(offsetMove)>=0.001f)
                    {
                        if (offsetMove < 0)
                        {
                            theScale.x = -Mathf.Abs(theScale.x);
                        }
                        else if (offsetMove > 0)
                        {
                            theScale.x = Mathf.Abs(theScale.x);
                        }
                    } 
                    objCharacterBody.transform.localScale = theScale;
                }
            }
            else
            {
                //是否在计算路径
                if (!navMeshAgent.pathPending)
                    SetAnimStatus(0);
            }
            mLastPosition = objMove.transform.position;
        }
    }

    /// <summary>
    /// 自动移动
    /// </summary>
    /// <param name="position">目的地</param>
    public bool SetDestination(Vector2 position)
    {
        isManualMove = false;
        bool canGo = true;
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
        {
            if (characterRigidbody != null)
            {
                characterRigidbody.inertia = 0;
            }
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
        Vector3 worldPos = tfPar.TransformPoint(position);
        return SetDestination(worldPos);
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        if (navMeshAgent != null)
            navMeshAgent.speed = moveSpeed;
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
        SetAnimStatus(1);
        //转向
        if (objCharacterBody != null)
        {
            Vector3 theScale = objCharacterBody.transform.localScale;
            if (x < 0)
            {
                theScale.x = -Mathf.Abs(theScale.x);
            }
            else if (x > 0)
            {
                theScale.x = Mathf.Abs(theScale.x);
            }
            objCharacterBody.transform.localScale = theScale;
        }

        //Vector2 lerpPosition = Vector3.Lerp(Vector2.zero, new Vector2(x, y), lerpOffset);
        // transform.Translate(movePosition * moveSpeed * Time.deltaTime);
   
        Vector3 movePosition = objMove.transform.position + new Vector3(x * moveSpeed * Time.deltaTime, y * moveSpeed * Time.deltaTime);
        characterRigidbody.MovePosition(movePosition);
       
        BoundaryMove();
    }

    /// <summary>
    /// 不受时间缩放影响的移动
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveForUnscaled(float x, float y)
    {
        isManualMove = true;
        //停止自动寻路
        StopAutoMove();
        SetAnimStatus(1);
        //转向
        if (objCharacterBody != null)
        {
            Vector3 theScale = objCharacterBody.transform.localScale;
            if (x < 0)
            {
                theScale.x = -Mathf.Abs(theScale.x);
            }
            else if (x > 0)
            {
                theScale.x = Mathf.Abs(theScale.x);
            }
            objCharacterBody.transform.localScale = theScale;
        }
        Vector3 movePosition = objMove.transform.position + new Vector3(x * moveSpeed * Time.unscaledDeltaTime, y * moveSpeed * Time.unscaledDeltaTime);
        transform.position = movePosition;
        //characterRigidbody.MovePosition(movePosition);
        BoundaryMove();
    }

    /// <summary>
    /// 自动寻路
    /// </summary>
    /// <param name="movePosition"></param>
    public void Move(Vector3 movePosition)
    {
        SetAnimStatus(1);
        //转向
        Vector3 theScale = objCharacterBody.transform.localScale;
        if (movePosition.x - objCharacterBody.transform.position.x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (movePosition.x - objCharacterBody.transform.position.x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        objCharacterBody.transform.localScale = theScale;
        //transform.position = movePosition;
        BoundaryMove();
    }

    /// <summary>
    ///  边界处理
    /// </summary>
    public void BoundaryMove()
    {
        Vector3 newPosition = objMove.transform.position;
        if (maxMoveX != 0 && objMove.transform.position.x > maxMoveX)
        {
            newPosition.x = maxMoveX;
        }
        if (minMoveX != 0 && objMove.transform.position.x < minMoveX)
        {
            newPosition.x = minMoveX;
        }
        if (maxMoveY != 0 && objMove.transform.position.y > maxMoveY)
        {
            newPosition.y = maxMoveY;
        }
        if (minMoveY != 0 && objMove.transform.position.y < minMoveY)
        {
            newPosition.y = minMoveY;
        }
        objMove.transform.position = newPosition;
    }

    /// <summary>
    /// 设置动画状态
    /// </summary>
    /// <param name="status"></param>
    public void SetAnimStatus(int status)
    {
        if (mAnimState == 10 && status == 0)
        {
            //如果角色已经死亡 则IDLE为不动
            status = 10;
        }
        if (mAnimState == status)
            return;
        mAnimState = status;
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("Status", mAnimState);
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
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled && Mathf.Abs(navMeshAgent.remainingDistance) > 0.1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 关闭自动寻路
    /// </summary>
    public void CloseNavMeshAgent()
    {
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;
    }

    /// <summary>
    /// 开启自动寻路
    /// </summary>
    public void OpenNavMeshAgent()
    {
        if (navMeshAgent != null)
            navMeshAgent.enabled = true;
    }
}