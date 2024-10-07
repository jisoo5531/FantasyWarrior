using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RockShootingState : IState
{
    private MonsterUnit monster;

    public M_RockShootingState(MonsterUnit monster)
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
        //Debug.Log("Idle State ���� ��");

        monster.GetComponent<BossAction>().RockShooting();
    }
    public void Exit()
    {
        //Debug.Log("Idle State Exit");
    }
}
