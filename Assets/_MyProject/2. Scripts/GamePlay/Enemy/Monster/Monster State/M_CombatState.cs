using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CombatState : IState
{
    private MonsterUnit monster;

    public M_CombatState(MonsterUnit monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        if (monster.damagable.Hp <= 0 || monster == null)
        {
            return;
        }
        //Debug.Log("Combat State Enter");
        if (false == monster.nav.isStopped)
        {
            monster.nav.isStopped = true;
        }
        
    }

    public void Excute()
    {
        //Debug.Log("Combat State 실행 중");

        monster.unitAnim.AttackAnimPlay();

        if (monster.followable.DistanceToPlayer > monster.attackable.Range)
        {
            //if (monster.unitAnim.IsPlayAnim())
            //{
            //    Debug.Log("아직 안 돼");
            //    return;
            //}
            if (monster.followable.DistanceToPlayer > monster.detectionRange)
            {
                monster.M_StateMachine.StateTransition(monster.M_StateMachine.followState);
                return;
            }
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.idleState);
        }
    }

    public void Exit()
    {
        //Debug.Log("Combat State exit");        
    }
}
