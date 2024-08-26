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
        Debug.Log("Follow State ���� ��");

        // TODO : follow state���� idle state�� ��ȯ �� ���� �ڸ��� �̵�?
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
