using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : NetworkBehaviour
{
    // 몬스터 데이터를 서버에서 받아오기 위해 SyncVar 사용
    [SyncVar] protected MonsterData monsterData;

    public MonsterStateMachine M_StateMachine;
    public PlayerController player;
    public UnitAnimation unitAnim;
    public NavMeshAgent nav;

    [HideInInspector] public MonsterDamagable damagable;
    [HideInInspector] public MonsterAttackable attackable;
    [HideInInspector] public Followable followable;

    protected MonsterUI monsterUI;

    [Tooltip("모든 몬스터의 공통된 탐지거리값")]
    public int detectionRange = 10;

    protected int monsterID;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
        Debug.Log($"플레이어 세팅 : {player.userID}");
    }

    private void Awake()
    {
        attackable = gameObject.AddComponent<MonsterAttackable>();
        damagable = gameObject.AddComponent<MonsterDamagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<MonsterUI>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();

    }

    private void OnEnable()
    {
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
        
    }

    

    public void ServerInit()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        monsterData = MonsterManager.Instance.GetMonsterData(monsterID);        
        if (monsterData == null)
        {
            Debug.LogError("몬스터 데이터가 없습니다.");
            return;
        }        

        damagable.Initialize(unitID: monsterData.MonsterID, maxHp: monsterData.MaxHp, hp: monsterData.Hp, isMonster: true);
        attackable.Initialize(damage: monsterData.Damage, range: monsterData.AttackRange);
        followable.Initialize(moveSpeed: monsterData.MoveSpeed);
        nav.speed = followable.MoveSpeed;

        monsterUI.Initialize(damagable);
        M_StateMachine = new MonsterStateMachine(this);        
        M_StateMachine.Initialize(M_StateMachine.idleState);
       
    }

    private void Update()
    {
        if (damagable.isStunned)
        {
            M_StateMachine.StateTransition(M_StateMachine.idleState);
            return;
        }
        if (damagable.Hp > 0)
        {            
            M_StateMachine.Excute();
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            followable.CalculateDistance(transform.position, player.transform.position);
        }
    }

    private void OnDisable()
    {
        damagable.OnTakeDamage -= OnHpChange;
        damagable.OnDeath -= OnDeath;
    }

    protected virtual void OnHpChange(int damage)
    {
        damagable.Hp -= damage;
    }

    protected virtual void OnDeath()
    {
        unitAnim.DeathAnimPlay();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3f);
    }
}
