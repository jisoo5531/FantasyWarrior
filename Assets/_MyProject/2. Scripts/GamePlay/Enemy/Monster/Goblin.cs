using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{            
    private void Awake()
    {        
        player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        damagable.OnHpChangeEvent += OnHpChange;

        Initialize();
    }
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��

        damagable.Initialize(maxHp: 100, hp: 100);
        attackable.Initialize(damage: 10, range: 2);
        followable.Initialize(moveSpeed: 1.5f);        

        nav.speed = followable.MoveSpeed;        
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
        followable.CalculateDistance(transform.position, player.transform.position);        
    }
}