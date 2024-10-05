using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : NetworkBehaviour
{
    // ���� �����͸� �������� �޾ƿ��� ���� SyncVar ���
    [SyncVar] protected MonsterData monsterData;

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

    }

    private void OnEnable()
    {
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;

        Debug.LogError("���� ������ " + isLocalPlayer);
        if (isLocalPlayer)
        {
            CmdRequestMonsterData();
        }        
    }

    // �������� ���� �����͸� ��û�ϴ� Command
    [Command]
    private void CmdRequestMonsterData()
    {
        Debug.LogError(MonsterManager.Instance == null);
        // �������� ���� �����͸� ������
        monsterData = MonsterManager.Instance.GetMonsterData(monsterID);

        // �������� ������ �����͸� Ŭ���̾�Ʈ�� ����ȭ
        RpcSetMonsterData(monsterData);
    }

    // �������� ���� �����͸� Ŭ���̾�Ʈ�� �����ϴ� ClientRpc
    [ClientRpc]
    private void RpcSetMonsterData(MonsterData data)
    {
        monsterData = data;
        Initialize();
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
            Debug.LogError("���� �����Ͱ� �����ϴ�.");
            return;
        }
        Debug.LogError(monsterData.MonsterName);

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
