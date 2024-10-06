//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 스턴 효과
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
//        target.isStunned = false; // 스턴 해제
//    }

//    private IEnumerator StunCoroutine(Damagable target)
//    {
//        target.isStunned = true; // 스턴 적용
//        yield return new WaitForSeconds(duration);
//        Remove(target); // 스턴 해제
//    }
//}
