using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public IState CurrentState { get; private set; }

    public M_IdleState idleState;
    public M_FollowState followState;
    public M_CombatState combatState;

    public MonsterStateMachine(MonsterUnit monster)
    {
        this.idleState = new M_IdleState(monster);
        this.followState = new M_FollowState(monster);
        this.combatState = new M_CombatState(monster);
    }
    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();        
    }

    public void StateTransition(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

    }

    public void Excute()
    {
        Debug.Log(CurrentState.ToString());
        CurrentState?.Excute();
    }
}
