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

    //是否是手动
    public bool isManualMove = false;
    private void FixedUpdate()
    {
        if (!isManualMove && navMeshAgent != null)
        {
            if (navMeshAgent.hasPath)
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
    public void SetDestination(Vector3 position)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.updatePosition = false;
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.SetDestination(position);

            if (characterAnimtor != null)
            {
                characterAnimtor.SetInteger("State", 1);
            }
        }
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
        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 1);
        }
        //转向
        Vector3 theScale = transform.localScale;
        if (x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        transform.localScale = theScale;
        Vector3 movePosition = Vector3.Lerp(Vector3.zero, new Vector3(x, y), lerpOffset);
        transform.Translate(movePosition * moveSpeed * Time.deltaTime);
    }


    public void Move(Vector3 movePosition)
    {
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 1);
        }
        //转向
        Vector3 theScale = transform.localScale;
        if (movePosition.x - transform.position.x < 0)
        {
            theScale.x = -Mathf.Abs(theScale.x);
        }
        else if (movePosition.x - transform.position.x > 0)
        {
            theScale.x = Mathf.Abs(theScale.x);
        }
        transform.localScale = theScale;
        Vector3 newMovePosition = Vector3.Lerp(transform.position, movePosition, lerpOffset);
        transform.position = newMovePosition;
    }

    public void Stop()
    {
        isManualMove = false;
        if (characterAnimtor != null)
        {
            characterAnimtor.SetInteger("State", 0);
        }
    }
}