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

        // TODO : 몬스터 능력치 나중에 따로 데이터베이스로 관리하여 데이터 받아와야 함

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