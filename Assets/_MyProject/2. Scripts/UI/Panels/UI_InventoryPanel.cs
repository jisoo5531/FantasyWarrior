using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPanel : MonoBehaviour
{
    [Header("Equip")]
    public Button EquipButton;
    public GameObject EquipContent;

    [Header("Consump")]
    public Button ConsumpButton;
    public GameObject ConsumpContent;

    [Header("Other")]
    public Button OtherButton;
    public GameObject OtherContent;

    private int itemID;

    private void Awake()
    {
        EquipButton.onClick.AddListener
            (
                () =>
                {
                    EquipContent.transform.parent.parent.gameObject.SetActive(true);
                    ConsumpContent.transform.parent.parent.gameObject.SetActive(false);
                    OtherContent.transform.parent.parent.gameObject.SetActive(false);
                }
            );
        ConsumpButton.onClick.AddListener
            (
                () =>
                {
                    ConsumpContent.transform.parent.parent.gameObject.SetActive(true);
                    EquipContent.transform.parent.parent.gameObject.SetActive(false);
                    OtherContent.transform.parent.parent.gameObject.SetActive(false);
                }
            );
        OtherButton.onClick.AddListener
            (
                () =>
                {
                    OtherContent.transform.parent.parent.gameObject.SetActive(true);
                    ConsumpContent.transform.parent.parent.gameObject.SetActive(false);
                    EquipContent.transform.parent.parent.gameObject.SetActive(false);
                }
            );
    }
    private void Start()
    {
        InventoryManager.Instance.OnGetItem += SetItemToSlot;
        
        if (InventoryManager.Instance.inventoryDataList.Count > 0)
        {
            Debug.Log("왜 안돼?");
            SetItemToSlot();
        }        
    }
    private void SetItemToSlot()
    {
        int equipAmount = 0;
        int consumpAmount = 0;
        int otherAmount = 0;
        foreach (InventoryData item in InventoryManager.Instance.inventoryDataList)
        {                                    
            this.itemID = item.Item_ID;
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);
            Item_Type itemType = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            int itemQuantity = item.Quantity;

            switch (itemType)
            {
                case Item_Type.Equipment:
                    SetItemToEquipSlot(sprite, itemQuantity, equipAmount++);
                    break;
                case Item_Type.Consump:
                    SetItemToConsumpSlot(sprite, itemQuantity, consumpAmount++);
                    break;
                case Item_Type.Other:
                    SetItemToOtherSlot(sprite, itemQuantity, otherAmount++);
                    break;
                default:
                    break;
            }
        }
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    if (i >= InventoryManager.Instance.inventoryDataList.Count)
        //    {
        //        continue;
        //    }

        //    int itemID = InventoryManager.Instance.inventoryDataList[i].Item_ID;
        //    string itemName = InventoryManager.Instance.GetInventoryItemNameFromDB(itemID);
        //    Item_Type itemType = InventoryManager.Instance.GetInventoryItemTypeFromDB(itemID);
        //    Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
        //    int itemQuantity = InventoryManager.Instance.inventoryDataList[i].Quantity;

        //    switch (itemType)
        //    {
        //        case Item_Type.Equipment:
        //            SetItemToEquipSlot();
        //            break;
        //        case Item_Type.Consump:
        //            SetItemToConsumpSlot();
        //            break;
        //        case Item_Type.Other:
        //            SetItemToOtherSlot();
        //            break;
        //        default:
        //            break;
        //    }

        //    slots[i].Initialize(sprite, itemQuantity);
        //}
    }
    private void SetItemToEquipSlot(Sprite sprite, int itemQuantity, int index)
    {        
        Debug.Log("여기>?");
        UI_InventorySlot slot = EquipContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Equipment, sprite, itemQuantity);
    }
    private void SetItemToConsumpSlot(Sprite sprite, int itemQuantity, int index)
    {
        Debug.Log("여기>????/");
        UI_InventorySlot slot = ConsumpContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Consump, sprite, itemQuantity);
    }
    private void SetItemToOtherSlot(Sprite sprite, int itemQuantity, int index)
    {
        Debug.Log("여기>?!!!!!!");
        UI_InventorySlot slot = OtherContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Other, sprite, itemQuantity);
    }
}
