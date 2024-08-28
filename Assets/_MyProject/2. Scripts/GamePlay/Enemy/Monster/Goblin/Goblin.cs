using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{                
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��
        damagable.Initialize(maxHp: 200, hp: 200);
        attackable.Initialize(damage: 10, range: 2);
        followable.Initialize(moveSpeed: 1.5f);        

        Debug.Log(nav == null);
        nav.speed = followable.MoveSpeed;                      
    }

    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    protected override void OnDeath()
    {
        Debug.Log("����?");
        GameObject item = Instantiate(GameManager.Instance.Item, transform.position, GameManager.Instance.Item.transform.rotation);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 40f, ForceMode.Impulse);

        base.OnDeath();
    }
}