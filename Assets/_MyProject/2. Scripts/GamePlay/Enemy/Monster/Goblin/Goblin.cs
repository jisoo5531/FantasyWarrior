using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goblin : MonsterUnit
{
    int rewardExp = 1000;
    protected override void Initialize()
    {
        monsterID = 1;
        base.Initialize();      
                    
    }

    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    // TODO : ���Ͱ� �׾��� �� ������, ����ġ, ��� ��� ���� ��������
    // TODO : ������ �����ϰ�?
    protected override void OnDeath()
    {
        nav.isStopped = true;
        ItemData rewardItem = ItemManager.Instance.GetItemData(8);

        GameObject rewardItemOBJ = Resources.Load<GameObject>($"Items/{rewardItem.Item_Name}");
        GameObject itemObj = Instantiate(rewardItemOBJ, transform.position + new Vector3(0, 0.5f, 0), rewardItemOBJ.transform.rotation);
        Item item = itemObj.GetComponent<Item>();
        item?.Initialize(ItemManager.Instance.GetItemData(1));
        UserStatManager.Instance.UpdateExp(rewardExp);
        //item.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);

        base.OnDeath();
    }
}