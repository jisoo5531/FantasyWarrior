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

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��
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
