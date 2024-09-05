using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPanel : MonoBehaviour
{
    public GameObject inventoryContent;   

    private void Awake()
    {
        
    }
    private void Start()
    {
        InventoryManager.Instance.OnGetItem += SetItemToSlot;

        Debug.Log(InventoryManager.Instance.inventoryDataList.Count);
        if (InventoryManager.Instance.inventoryDataList.Count > 0)
        {
            Debug.Log("¿Ö ¾ÈµÅ?");
            SetItemToSlot();
        }        
    }
    private void SetItemToSlot()
    {
        Debug.Log("¿©±â>?");
        UI_InventorySlot[] slots = inventoryContent.GetComponentsInChildren<UI_InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {                       
            if (i >= InventoryManager.Instance.inventoryDataList.Count)
            {
                continue;
            }
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{InventoryManager.Instance.GetInventoryItemNameFromDB(InventoryManager.Instance.inventoryDataList[i].Item_ID)}");
            int itemQuantity = InventoryManager.Instance.inventoryDataList[i].Quantity;
            slots[i].Initialize(sprite, itemQuantity);
        }
    }
}
