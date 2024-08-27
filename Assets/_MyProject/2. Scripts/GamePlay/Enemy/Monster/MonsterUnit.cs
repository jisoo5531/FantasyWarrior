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

    [Tooltip("모든 몬스터의 공통된 탐지거리값")]
    public int detectionRange = 10;

    private void Start()
    {
        Debug.Log("부모 Start");
    }

    protected virtual void Initialize()
    {
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();        
    }
    private void OnEnable()
    {
        damagable.OnHpChangeEvent += OnHpChange;
        damagable.OnDeathEvent += OnDeath;
    }

    protected void OnHpChange(int damage)
    {
        damagable.Hp -= damage;
        Debug.Log($"데미지 받음 {damage} 만큼");
    }
    

    private void OnDisable()
    {
        Debug.Log("유닛 비활성");
        damagable.OnHpChangeEvent -= OnHpChange;
        damagable.OnDeathEvent -= OnDeath;
    }
    //private void OnDestroy()
    //{
    //    Debug.Log("유닛 파괴");
    //    damagable.OnHpChangeEvent -= OnHpChange;
    //    damagable.OnDeathEvent -= OnDeath;
    //}

    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        
        Destroy(gameObject, 3f);
    }
}