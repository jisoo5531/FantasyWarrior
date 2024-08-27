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

    private void Start()
    {
        Debug.Log("�θ� Start");
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
        Debug.Log($"������ ���� {damage} ��ŭ");
    }
    

    private void OnDisable()
    {
        Debug.Log("���� ��Ȱ��");
        damagable.OnHpChangeEvent -= OnHpChange;
        damagable.OnDeathEvent -= OnDeath;
    }
    //private void OnDestroy()
    //{
    //    Debug.Log("���� �ı�");
    //    damagable.OnHpChangeEvent -= OnHpChange;
    //    damagable.OnDeathEvent -= OnDeath;
    //}

    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        
        Destroy(gameObject, 3f);
    }
}