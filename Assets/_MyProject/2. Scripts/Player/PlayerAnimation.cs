using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveAnimation(float speed, bool isRun)
    {
        speed = isRun ? speed * 2 : speed;
        anim.SetFloat("Speed", speed);
    }

    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }
}
