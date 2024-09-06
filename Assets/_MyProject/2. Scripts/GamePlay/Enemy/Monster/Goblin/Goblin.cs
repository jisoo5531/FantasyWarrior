using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goblin : MonsterUnit
{                    
    protected override void Initialize()
    {
        unitName = "Goblin";
        base.Initialize();      
                    
    }

    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    protected override void OnDeath()
    {
        Debug.Log("Á×À½?");
        Debug.Log(ItemManager.Instance.itemDataList == null);
        Debug.Log(ItemManager.Instance.itemDataList[0].Item_Name);
        GameObject rewardItem = Resources.Load<GameObject>($"Items/{ItemManager.Instance.itemDataList[0].Item_Name}");
        GameObject itemObj = Instantiate(rewardItem, transform.position + new Vector3(0, 0.5f, 0), rewardItem.transform.rotation);
        Item item = itemObj.GetComponent<Item>();
        item?.Initialize(ItemManager.Instance.itemDataList[2]);
        //item.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);

        base.OnDeath();
    }
}