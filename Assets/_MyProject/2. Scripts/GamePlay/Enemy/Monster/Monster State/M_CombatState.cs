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
        //Debug.Log("Combat State Enter");
        if (false == monster.nav.isStopped)
        {
            monster.nav.isStopped = true;
        }
    }

    public void Excute()
    {
        //Debug.Log("Combat State ½ÇÇà Áß");
        if (monster.followable.DistanceToPlayer > monster.attackable.Range)
        {
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
