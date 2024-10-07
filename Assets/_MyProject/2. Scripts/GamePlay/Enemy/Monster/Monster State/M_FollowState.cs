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
        if (monster.damagable.Hp <= 0 || monster == null)
        {
            return;
        }
        //Debug.Log("Follow State Enter");

        monster.nav.isStopped = false;
        
        monster.unitAnim.MoveAnimPlay(true);
    }

    public void Excute()
    {
        //Debug.Log("Follow State ���� ��");        
        monster.nav?.SetDestination(monster.player.transform.position);

        // TODO : follow state���� idle state�� ��ȯ �� ���� �ڸ��� �̵�?
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
        monster.unitAnim.MoveAnimPlay(false);
        monster.nav.isStopped = true;
        monster.nav.ResetPath();
    }
}
