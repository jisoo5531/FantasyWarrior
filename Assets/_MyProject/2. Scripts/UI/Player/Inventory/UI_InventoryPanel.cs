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

        if (InventoryManager.Instance.inventoryDataList.Count > 0)
        {
            SetItemToSlot();
        }        
    }
    private void SetItemToSlot()
    {
        for (int i = 0; i < InventoryManager.Instance.inventoryDataList.Count; i++)
        {            
            UI_InventorySlot slot = inventoryContent.transform.GetChild(i).gameObject.AddComponent<UI_InventorySlot>();            
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{InventoryManager.Instance.GetInventoryItemNameFromDB(InventoryManager.Instance.inventoryDataList[i].Item_ID)}");
            Dictionary<Image, Sprite> imgParam = new Dictionary<Image, Sprite>
            {
                { slot.transform.GetChild(1).GetComponent<Image>(), sprite }                
            };            
            int itemQuantity = InventoryManager.Instance.inventoryDataList[i].Quantity;
            Dictionary<TMPro.TMP_Text, string> txtParam = new Dictionary<TMPro.TMP_Text, string>
            {
                { slot.transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>(), itemQuantity.ToString() }
            };
            slot.Initialize(imgParam, txtParam);                            
        }
    }
}
