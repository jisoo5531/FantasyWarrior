using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goblin : MonsterUnit
{                    
    protected override void Initialize()
    {
        base.Initialize();      
                    
    }

    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    protected override void OnDeath()
    {
        Debug.Log("Á×À½?");
        GameObject item = Instantiate(GameManager.Instance.Item, transform.position, GameManager.Instance.Item.transform.rotation);
        item.GetComponent<Rigidbody>().AddForce(Vector3.up * 40f, ForceMode.Impulse);

        base.OnDeath();
    }

    protected override void GetFromDatabaseData()
    {
        whereQuery = new Dictionary<string, object>
        {
            { "name", "Goblin" }
        };
        base.GetFromDatabaseData();
    }
}