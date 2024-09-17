using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goblin : MonsterUnit
{
    int rewardExp = 1000;
    protected override void Initialize()
    {
        unitName = "Goblin";
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
        List<ItemData> itemDataList = ItemManager.Instance.itemDataList;
        GameObject rewardItem = Resources.Load<GameObject>($"Items/{itemDataList[7].Item_Name}");
        GameObject itemObj = Instantiate(rewardItem, transform.position + new Vector3(0, 0.5f, 0), rewardItem.transform.rotation);
        Item item = itemObj.GetComponent<Item>();
        item?.Initialize(itemDataList[0]);
        UserStatManager.Instance.UpdateExp(rewardExp);
        //item.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);

        base.OnDeath();
    }
}