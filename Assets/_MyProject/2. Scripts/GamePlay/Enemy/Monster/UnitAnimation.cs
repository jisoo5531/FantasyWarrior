using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private List<AnimationClip> attackClips;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveAnimPlay(bool isTrue)
    {
        anim.SetBool("IsMove", isTrue);
    }
    public void AttackAnimPlay()
    {
        if (IsAttackAnimPlay())
        {
            return;
        }
        anim.SetTrigger("Attack");
    }

    public bool IsAttackAnimPlay()
    {
        AnimatorClipInfo[] currentClipInfo = anim.GetCurrentAnimatorClipInfo(1);

        foreach (var clipInfo in currentClipInfo)
        {
            // ���� ��� ���� Ŭ���� attackClips ����Ʈ�� Ŭ���� ���մϴ�.
            if (attackClips.Contains(clipInfo.clip))
            {
                return true;
            }
        }
        return false;
    }
    
    public void DeathAnimPlay()
    {
        anim.SetLayerWeight(1, 0);
        anim.SetTrigger("Death");
    }
}