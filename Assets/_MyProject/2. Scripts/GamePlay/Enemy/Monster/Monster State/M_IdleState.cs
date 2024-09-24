using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IdleState : IState
{
    private MonsterUnit monster;

    public M_IdleState(MonsterUnit monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        if (monster.damagable.Hp <= 0 || monster == null)
        {
            return;
        }
        //Debug.Log("Idle State Enter");        

    }
    public void Excute()
    {
        if (monster.damagable.Hp <= 0 || monster == null)
        {
            return;
        }
        //Debug.Log("Idle State ½ÇÇà Áß");

        if (monster.followable.DistanceToPlayer <= monster.detectionRange)
        {
            if (monster.followable.DistanceToPlayer <= monster.attackable.Range)
            {
                monster.M_StateMachine.StateTransition(monster.M_StateMachine.combatState);
                return;
            }
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.followState);
        }
    }
    public void Exit()
    {
        //Debug.Log("Idle State Exit");
    }
}
