using UnityEngine;
using UnityEditor;

public class NpcAICustomerCpt : BaseNpcAI
{
    public Vector3 endPosition;

    public void SetEndPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
        characterMoveCpt.SetDestination(endPosition);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, endPosition)<3)
        {
            Destroy(gameObject);
        }
    }
}