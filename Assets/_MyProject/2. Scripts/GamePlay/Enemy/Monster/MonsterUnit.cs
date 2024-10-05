using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : NetworkBehaviour
{
    // TODO : ���͸� ������ ����(����ġ, ��ȭ, ������) ���� ��´�.    
    protected MonsterData monsterData;

    public MonsterStateMachine M_StateMachine;
    public PlayerController player;
    public UnitAnimation unitAnim;    
    public NavMeshAgent nav;

    [HideInInspector] public Damagable damagable;
    [HideInInspector] public Attackable attackable;
    [HideInInspector] public Followable followable;

    protected UIComponent monsterUI;    

    [Tooltip("��� ������ ����� Ž���Ÿ���")]
    public int detectionRange = 10;

    protected int monsterID;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
        Debug.Log($"�÷��̾� ���� : {player.userID}");
    }

    private void Awake()
    {        
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<UIComponent>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();
        Debug.Log("����");
    }

    private void OnEnable()
    {
        Debug.Log("����");
        Initialize();
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
    }
    protected virtual void Initialize()
    {
        Debug.Log("����" + monsterID);
        Debug.Log(MonsterManager.Instance == null);
        Debug.Log(MonsterManager.Instance.GetMonsterData(monsterID).MonsterName);

        this.monsterData = MonsterManager.Instance.GetMonsterData(monsterID);
        Debug.Log("����");
        Debug.Log(monsterData.MonsterName);
        damagable.Initialize(unitID: monsterData.MonsterID, maxHp: monsterData.MaxHp, hp: monsterData.Hp, isMonster: true);
        attackable.Initialize(damage: monsterData.Damage, range: monsterData.AttackRange);
        followable.Initialize(moveSpeed: monsterData.MoveSpeed);
        nav.speed = followable.MoveSpeed;

        monsterUI.Initialize(damagable);
        Debug.Log("����");
        M_StateMachine = new MonsterStateMachine(this);
        Debug.Log("����");
        M_StateMachine.Initialize(M_StateMachine.idleState);
        Debug.Log("����");
    }
    private void Update()
    {
        if (damagable.isStunned)
        {
            Debug.Log("�� �����̴�. �� ������");
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
    //    Debug.Log("���� �ı�");
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}
    
    protected virtual void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;        
    }
    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3f);
    }
}