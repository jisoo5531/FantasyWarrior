using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonsterUnit
{                
    protected override void Initialize()
    {
        base.Initialize();

        // TODO : 몬스터 능력치 나중에 따로 데이터베이스로 관리하여 데이터 받아와야 함
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
        Debug.Log("죽음?");
        GameObject item = Instantiate(GameManager.Instance.Item, transform.position, GameManager.Instance.Item.transform.rotation);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 40f, ForceMode.Impulse);

        base.OnDeath();
    }
}