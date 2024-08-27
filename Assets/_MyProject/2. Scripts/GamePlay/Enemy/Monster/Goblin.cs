using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{        
    private void Awake()
    {
        Initialize();

        player = FindObjectOfType<PlayerController>();
        
    }
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��

        damagable = new Damagable(maxHp: 100, hp: 100);
        attackable = new Attackable(damage: 10, range: 2);
        followable = new Followable(moveSpeed: 1.5f);

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
        Debug.Log(followable.DistanceToPlayer);
    }
}
