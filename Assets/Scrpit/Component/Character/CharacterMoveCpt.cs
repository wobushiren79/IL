using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using Pathfinding;
public class CharacterMoveCpt : BaseMonoBehaviour
{
    protected Seeker aiSeeker;
    //移动差值
    public float lerpOffset = 0.9f;
    //移动速度
    public float moveSpeed = 1;

    //角色动画
    public Animator characterAnimtor;

    //移动对象
    public GameObject objMove;
    public GameObject objCharacterBody;
    public Rigidbody2D characterRigidbody;

    //找到的路径
    public Path aiPath;
    public int aiPathPosition;
    public bool aiPathReached = false;

    //是否手动移动
    public bool isManualMove = false;

    public float minMoveX = 0;
    public float maxMoveX = 0;
    public float minMoveY = 0;
    public float maxMoveY = 0;

    private void Awake()
    {
        aiSeeker = GetComponent<Seeker>();
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


    public void Update()
    {
        if (!isManualMove && aiPath != null )
        {
            if(aiPathPosition >= aiPath.vectorPath.Count)
            {
                InitAIPath();
                SetAnimStatus(0);
                return;
            }
            else
            {
                aiPathReached = true;
                bool isArrive = Move(aiPath.vectorPath[aiPathPosition]);
                if (isArrive)
                {
                    aiPathPosition ++ ;
                }
                if (objCharacterBody != null)
                {
                    Vector3 theScale = objCharacterBody.transform.localScale;
                    float offsetMove = objCharacterBody.transform.position.x - mLastPosition.x;
                    if (Mathf.Abs(offsetMove) >= 0.001f)
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
            mLastPosition = objMove.transform.position;
        }
    }


    private void FixedUpdate()
    {
       
    }

    /// <summary>
    /// 自动移动
    /// </summary>
    /// <param name="position">目的地</param>
    public void SetDestination(Vector2 position)
    {
        isManualMove = false;
        if (aiSeeker != null)
        {
            if (characterRigidbody != null)
            {
                characterRigidbody.inertia = 0;
            }
            aiPath = null;
            aiPathPosition = 0;
            aiPathReached = true;
            aiSeeker.StartPath(objMove.transform.position, position, OnPathComplete);
        }
    }

    public void SetFollower()
    {

    }

    /// <summary>
    /// 路径找寻完毕
    /// </summary>
    /// <param name="path"></param>
    public void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            aiPath = path;
            aiPathPosition = 0;
        }
        else
        {
            InitAIPath();
            LogUtil.LogWarning("路径查询失败");
        }
    

    }

    protected void InitAIPath()
    {
        aiPath = null;
        aiPathPosition = 0;
        aiPathReached = false;
    }

    /// <summary>
    /// 本地移动
    /// </summary>
    /// <param name="tfPar"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public void SetDestinationLocal(Transform tfPar, Vector3 position)
    {
        Vector3 worldPos = tfPar.TransformPoint(position);
        SetDestination(worldPos);
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
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
    public bool Move(Vector3 movePosition)
    {
        SetAnimStatus(1);
        //objMove.transform.position = Vector3.Lerp(objMove.transform.position, movePosition, moveSpeed * Time.deltaTime);
        objMove.transform.position = Vector3.MoveTowards(objMove.transform.position , movePosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(objMove.transform.position, movePosition) < 0.01f)
        {
            objMove.transform.position = movePosition;
            return true;
        }
        else
        {
            return false;
        }

        //navMeshAgent.transform.position = movePosition;
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
        InitAIPath();
    }

    /// <summary>
    /// 自动寻路是否停止
    /// </summary>
    /// <returns></returns>
    public bool IsAutoMoveStop()
    {
        if (aiPath != null ||  aiPathReached)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


}