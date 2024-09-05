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
            Debug.Log("Ÿ�� ���̾� �ƴ�");
            return;
        }
        Debug.Log("�÷��̾� ������ �Ծ���.");
        InventoryManager.Instance.GetItem_InsertDatabase(itemData, 2);

        Destroy(gameObject);
    }
    
}
