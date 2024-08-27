using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class M_FollowState : IState
{
    private MonsterUnit monster;    

    public M_FollowState(MonsterUnit monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        //Debug.Log("Follow State Enter");
        if (monster.nav.isStopped)
        {
            monster.nav.isStopped = false;
        }
    }

    public void Excute()
    {
        //Debug.Log("Follow State 실행 중");
        
        monster.nav?.SetDestination(monster.player.transform.position);        

        // TODO : follow state에서 idle state로 전환 시 원래 자리로 이동?
        if (monster.followable.DistanceToPlayer >= monster.detectionRange)
        {
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.idleState);
        }
        else if (monster.followable.DistanceToPlayer <= monster.attackable.Range)
        {
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.combatState);
        }
    }

    public void Exit()
    {
        //Debug.Log("Follow State exit");
    }
}
