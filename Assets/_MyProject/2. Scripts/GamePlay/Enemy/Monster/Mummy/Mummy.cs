using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonsterUnit
{
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : ���� �ɷ�ġ ���߿� ���� �����ͺ��̽��� �����Ͽ� ������ �޾ƿ;� ��
        damagable.Initialize(maxHp: 100, hp: 100);
        attackable.Initialize(damage: 8, range: 0.7f);
        followable.Initialize(moveSpeed: 1f);

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
