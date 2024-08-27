using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : Enemy
{
    public MonsterStateMachine M_StateMachine;
    public PlayerController player;
    public NavMeshAgent nav;
    public UnitAnimation unitAnim;

    [HideInInspector] public Damagable damagable;
    [HideInInspector] public Attackable attackable;
    [HideInInspector] public Followable followable;

    [Tooltip("��� ������ ����� Ž���Ÿ���")]
    public int detectionRange = 10;

    protected virtual void Initialize()
    {
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();
    }

    protected void OnHpChange(int damage)
    {
        damagable.Hp -= damage;
        Debug.Log($"������ ���� {damage} ��ŭ");
    }
}