//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// ���� ȿ��
///// </summary>
//public class StunEffect : IStatusEffect
//{
//    private float duration;

//    public StunEffect(float stunDuration)
//    {
//        duration = stunDuration;
//    }

//    public void Apply(Damagable target)
//    {
//        target.StartCoroutine(StunCoroutine(target));
//    }

//    public void Remove(Damagable target)
//    {
//        target.isStunned = false; // ���� ����
//    }

//    private IEnumerator StunCoroutine(Damagable target)
//    {
//        target.isStunned = true; // ���� ����
//        yield return new WaitForSeconds(duration);
//        Remove(target); // ���� ����
//    }
//}
