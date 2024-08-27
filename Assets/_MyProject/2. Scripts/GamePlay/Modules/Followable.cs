using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followable : MonoBehaviour, IFollowable
{
    public float MoveSpeed { get; set; }
    public float DistanceToPlayer { get; set; }

    public Followable(float moveSpeed)
    {
        this.MoveSpeed = moveSpeed;
    }

    public void CalculateDistance(Vector3 originPos, Vector3 targetPos)
    {
        DistanceToPlayer = Vector3.Distance(targetPos, originPos);
    }
}
