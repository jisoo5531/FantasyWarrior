using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : Enemy
{
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

    protected string unitName;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<UIComponent>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();

        Initialize();
    }
    protected virtual void Initialize()
    {
        
    }

    private void Start()
    {
        GetFromDatabaseData();        
        damagable.Initialize(maxHp: monsterData.MaxHp, hp: monsterData.Hp);
        attackable.Initialize(damage: monsterData.Damage, range: monsterData.AttackRange);
        followable.Initialize(moveSpeed: monsterData.MoveSpeed);
        nav.speed = followable.MoveSpeed;

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
        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;    
    }
    private void OnDisable()
    {        
        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;
    }
    //private void OnDestroy()
    //{
    //    Debug.Log("���� �ı�");
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}

    /// <summary>
    /// �����ͺ��̽����� ���� ������ ��������
    /// </summary>
    protected void GetFromDatabaseData()
    {
        // TODO : ���� ������ �ٲٱ�
        string query =
            $"SELECT *\n" +
            $"FROM monsters\n" +
            $"WHERE NAME='{unitName}';";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];

            monsterData = new MonsterData(row);
        }
        else
        {
            //  ����

        }
    }
    
    protected virtual void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;
        Debug.Log($"������ ���� {damage} ��ŭ");
    }
    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        
        Destroy(gameObject, 3f);
    }
}
public class MonsterData
{
    public int MonsterID { get; set; }
    public string MonsterName { get; set; }    
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int Damage { get; set; }
    public int Defense { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackRange { get; set; }
    public int EXP_Reward { get; set; }
    public int Gold_Reward { get; set; }


    public MonsterData(DataRow row) : this
        (
            int.Parse(row["monster_id"].ToString()),
            row["name"].ToString(),
            int.Parse(row["maxhp"].ToString()),
            int.Parse(row["hp"].ToString()),
            int.Parse(row["damage"].ToString()),
            int.Parse(row["defense"].ToString()),
            float.Parse(row["move_speed"].ToString()),
            float.Parse(row["attack_range"].ToString()),
            int.Parse(row["experience_reward"].ToString()),
            int.Parse(row["gold_reward"].ToString())
        )
    { }

    public MonsterData(int monsterID, string monsterName, int maxHp, int hp, int damage, int defense, float moveSpeed, float attackRange, int eXP_Reward, int gold_Reward)
    {
        this.MonsterID = monsterID;
        this.MonsterName = monsterName;
        this.MaxHp = maxHp;
        this.Hp = hp;
        this.Damage = damage;
        this.Defense = defense;
        this.MoveSpeed = moveSpeed;
        this.AttackRange = attackRange;
        this.EXP_Reward = eXP_Reward;
        this.Gold_Reward = gold_Reward;
    }
}
