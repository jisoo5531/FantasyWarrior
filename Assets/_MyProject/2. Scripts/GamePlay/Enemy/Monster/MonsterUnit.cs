using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : Enemy
{
    // TODO : 몬스터를 잡으면 보상(경험치, 재화, 아이템) 등을 얻는다.    
    protected MonsterData monsterData;
    public MonsterStateMachine M_StateMachine;    

    // TODO : 플레이어 찾기 보스 임시 주석
    public GameObject player;
    //public PlayerController player;
    public UnitAnimation unitAnim;
    public NavMeshAgent nav;

    [HideInInspector] public Damagable damagable;
    [HideInInspector] public Attackable attackable;
    [HideInInspector] public Followable followable;

    protected UIComponent monsterUI;

    [Tooltip("모든 몬스터의 공통된 탐지거리값")]
    public int detectionRange = 10;

    protected int monsterID;

    private void Awake()
    {
        //player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<UIComponent>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        Initialize();
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
    }

    protected virtual void Initialize()
    {
        Debug.Log("몬스터 아이디 " + monsterID);
        if (this.monsterData == null)
        {
            this.monsterData = MonsterManager.Instance.GetMonsterData(monsterID);
        }
        Debug.Log(monsterData.MonsterName);
        damagable.Initialize(unitID: monsterData.MonsterID, maxHp: monsterData.MaxHp, hp: monsterData.Hp, isMonster: true);
        attackable.Initialize(damage: monsterData.Damage, range: monsterData.AttackRange);
        followable.Initialize(moveSpeed: monsterData.MoveSpeed);
        nav.speed = followable.MoveSpeed;

        monsterUI?.Initialize(damagable);
        M_StateMachine = new MonsterStateMachine(this);
        M_StateMachine.Initialize(M_StateMachine.idleState);
    }
    private void Update()
    {
        Debug.Log("nav " + nav.isStopped);
        if (damagable.isStunned)
        {
            Debug.Log("얘 스턴이다. 못 움직여");
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
        followable.CalculateDistance(transform.position, player.transform.position);
    }

    private void OnDisable()
    {
        damagable.OnTakeDamage -= OnHpChange;
        damagable.OnDeath -= OnDeath;
    }
    //private void OnDestroy()
    //{
    //    Debug.Log("유닛 파괴");
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}

    protected virtual void OnHpChange(int damage)
    {
        // TODO : 유닛마다 들어가는 데미지 다르게끔 (방어도? 따라)
        damagable.Hp -= damage;
    }
    protected virtual void OnDeath()
    {
        unitAnim.DeathAnimPlay();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3f);
    }

    public void IsNavStop(bool isStop, float afterTime)
    {
        if (isStop)
        {
            Invoke("NavStop", afterTime);
        }
        else
        {
            Invoke("NavMove", afterTime);
        }
    }
    private void NavStop()
    {
        nav.isStopped = true;
    }
    private void NavMove()
    {
        nav.isStopped = false;
    }
}