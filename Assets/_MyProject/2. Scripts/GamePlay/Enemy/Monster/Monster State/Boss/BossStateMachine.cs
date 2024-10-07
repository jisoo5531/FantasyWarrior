using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonsterStateMachine
{
    public M_JumpState jumpState;
    public M_RockShootingState rockShootingState;
    public M_SpinState spinState;
    public BossStateMachine(MonsterUnit monster) : base(monster)
    {
        this.jumpState = new M_JumpState(monster);
        this.rockShootingState = new M_RockShootingState(monster);
        this.spinState = new M_SpinState(monster);        
    }

}
