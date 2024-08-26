using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{
    public override int Damage { get; set; }
    
    private void Awake()
    {
        Initialize();

        player = FindObjectOfType<PlayerController>();
        
    }
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : 몬스터 능력치 나중에 따로 데이터베이스로 관리하여 데이터 받아와야 함
        MaxHp = 100;
        Hp = MaxHp;
        Range = 2;        
        nav.speed = MoveSpeed = 1.5f;
        
    }

    private void Start()
    {
        M_StateMachine = new MonsterStateMachine(this);
        M_StateMachine.Initialize(M_StateMachine.idleState);        
    }

    private void Update()
    {
        M_StateMachine.Excute();
        
    }
    private void LateUpdate()
    {
        CalculateDistance();
    }


    public override void SendDamage(int damage)
    {
        
    }
}
