using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_FollowState : IState
{
    private MonsterUnit monster;

    public M_FollowState(MonsterUnit monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        Debug.Log("Follow State Enter");
    }

    public void Excute()
    {
        Debug.Log("Follow State 실행 중");

        // TODO : follow state에서 idle state로 전환 시 원래 자리로 이동?
        if (monster.DistanceToPlayer >= monster.detectionRange)
        {
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.idleState);
        }
        else if (monster.DistanceToPlayer <= monster.Range)
        {
            monster.M_StateMachine.StateTransition(monster.M_StateMachine.combatState);
        }
    }

    public void Exit()
    {
        Debug.Log("Follow State exit");
    }
}
