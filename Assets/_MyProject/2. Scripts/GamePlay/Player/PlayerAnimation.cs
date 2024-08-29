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
    public void SkillAnimation(List<bool> skillInput, Dictionary<int, string> skillTable)
    {
        int skillNum = skillInput.IndexOf(true);
        if (skillNum < 0)
        {
            return;
        }
        Debug.Log($"{skillTable[skillNum]} »ç¿ë");
        anim.SetTrigger($"{skillTable[skillNum]}"); 
    }
    public void DeathAnimation()
    {
        anim.SetTrigger("Death");
    }
}
