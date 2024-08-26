using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{
    public override int Damage { get; set; }
    
    private void Awake()
    {
        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��
        MaxHp = 100;
        Hp = MaxHp;
        Range = 2;

        M_StateMachine = new MonsterStateMachine(this);
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        M_StateMachine.Initialize(M_StateMachine.idleState);        
    }

    private void Update()
    {
        M_StateMachine.Excute();
        CalculateDistance();
    }



    public override void SendDamage(int damage)
    {
        
    }
}
