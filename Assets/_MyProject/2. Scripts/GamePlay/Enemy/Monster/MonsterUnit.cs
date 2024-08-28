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

    protected MonsterUI monsterUI;

    [Tooltip("모든 몬스터의 공통된 탐지거리값")]
    public int detectionRange = 10;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<MonsterUI>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();

        Initialize();
    }
    protected virtual void Initialize()
    {
        
    }

    private void Start()
    {        
        monsterUI.Initialize(damagable);        
        M_StateMachine = new MonsterStateMachine(this);
        M_StateMachine.Initialize(M_StateMachine.idleState);
    }

    private void Update()
    {
        M_StateMachine.Excute();

    }
    private void LateUpdate()
    {
        followable.CalculateDistance(transform.position, player.transform.position);
    }
    
    private void OnEnable()
    {
        EventHandler.actionEvent.RegisterHpChange(OnHpChange);
        EventHandler.actionEvent.RegisterDeath(OnDeath);        
    }

    private void OnDisable()
    {
        Debug.Log("유닛 비활성");
        EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
        EventHandler.actionEvent.UnRegisterDeath(OnDeath);        
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
        Debug.Log($"데미지 받음 {damage} 만큼");
    }
    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        
        Destroy(gameObject, 3f);
    }
}