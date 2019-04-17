using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

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
    //是否手动移动
    public bool isManualMove=false;
    private void Start()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updatePosition = false;
        }
    }
    private void FixedUpdate()
    {
        if (!isManualMove&&navMeshAgent != null)
        {
            if (!navMeshAgent.isStopped && navMeshAgent.hasPath)
            {
                Move(navMeshAgent.nextPosition);
            }
            else
            {
                Stop();
            }
        }
       
    }

    /// <summary>
    /// 自动移动
    /// </summary>
    /// <param name="position">目的地</param>
    public bool SetDestination(Vector3 position)
    {
        isManualMove = false ;
        bool canGo = true;
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updatePosition = false;
            navMeshAgent.speed = moveSpeed;
            canGo = navMeshAgent.SetDestination(position);

            if (characterAnimtor != null)
            {
                characterAnimtor.SetInteger("State", 1);
            }
        }
        return canGo;
    }

    /// <summary>
    /// 获取路径状态
    /// </summary>
    /// <returns></returns>
    public NavMeshPathStatus GetAutoMovePathStatus()
    {
       return   navMeshAgent.pathStatus;
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
        Vector3 movePosition = Vector3.Lerp(Vector3.zero, new Vector3(x, y, 0), lerpOffset);
        transform.Translate(movePosition * moveSpeed * Time.deltaTime);
    }


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
        Vector3 newMovePosition = Vector3.Lerp(transform.position, movePosition, lerpOffset);
        transform.position = newMovePosition;
    }

    public void Stop()
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
        if (navMeshAgent != null && navMeshAgent.hasPath && !navMeshAgent.isStopped)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}