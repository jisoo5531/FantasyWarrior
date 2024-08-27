using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterUnit : Enemy
{
    public MonsterStateMachine M_StateMachine;
    public PlayerController player;
    public NavMeshAgent nav;

    [HideInInspector] public Damagable damagable;
    [HideInInspector] public Attackable attackable;
    [HideInInspector] public Followable followable;        

    [Tooltip("��� ������ ����� Ž���Ÿ���")]
    public int detectionRange = 10;
   
    protected virtual void Initialize()
    {
        nav = GetComponent<NavMeshAgent>();        
    }
}
