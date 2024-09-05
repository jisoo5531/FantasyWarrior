using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public LayerMask targetLayer;
    public ItemData itemData;

    public void Initialize(ItemData itemData)
    {
        this.itemData = itemData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            Debug.Log("타겟 레이어 아님");
            return;
        }
        Debug.Log("플레이어 아이템 먹었다.");
        InventoryManager.Instance.GetItem_InsertDatabase(itemData, 2);

        Destroy(gameObject);
    }
    
}
