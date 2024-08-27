using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonsterUnit
{
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        Initialize();
    }
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��
        damagable.Initialize(maxHp: 100, hp: 100);
        attackable.Initialize(damage: 8, range: 1.5f);
        followable.Initialize(moveSpeed: 0.8f);

        Debug.Log(nav == null);
        nav.speed = followable.MoveSpeed;
    }

    private void Start()
    {
        Debug.Log("�ڽ� Start");
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

    protected override void OnDeath()
    {
        Debug.Log("����?");
        GameObject item = Instantiate(GameManager.Instance.Item, transform.position, GameManager.Instance.Item.transform.rotation);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 40f, ForceMode.Impulse);

        base.OnDeath();
    }
}
