using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPanel : MonoBehaviour
{
    [Header("Equip")]
    public Button EquipTabButton;
    public GameObject EquipContent;

    [Header("Consump")]
    public Button ConsumpTabButton;
    public GameObject ConsumpContent;

    [Header("Other")]
    public Button OtherTabButton;
    public GameObject OtherContent;

    private int itemID;

    private void Awake()
    {
        EquipTabButton.onClick.AddListener
            (
                () =>
                {
                    EquipContent.transform.parent.parent.gameObject.SetActive(true);
                    ConsumpContent.transform.parent.parent.gameObject.SetActive(false);
                    OtherContent.transform.parent.parent.gameObject.SetActive(false);
                }
            );
        ConsumpTabButton.onClick.AddListener
            (
                () =>
                {
                    ConsumpContent.transform.parent.parent.gameObject.SetActive(true);
                    EquipContent.transform.parent.parent.gameObject.SetActive(false);
                    OtherContent.transform.parent.parent.gameObject.SetActive(false);
                }
            );
        OtherTabButton.onClick.AddListener
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
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        PlayerEquipManager.Instance.OnUnEquipItem += SetItemToSlot;
        InventoryManager.Instance.OnGetItem += SetItemToSlot;
        
        if (InventoryManager.Instance.inventoryDataList.Count > 0)
        {
            Debug.Log("왜 안돼?");
            SetItemToSlot();
        }        
    }
    /// <summary>
    /// <para>인벤토리 테이블에 저장된 아이템들을 인벤토리 슬롯으로 세팅</para>
    /// 아이템 획득, 장비 장착, 장비 해제 등등. 인벤토리가 업데이트되면 실행.
    /// </summary>
    private void SetItemToSlot()
    {
        int equipAmount = 0;
        int consumpAmount = 0;
        int otherAmount = 0;
        SlotClear(EquipContent);
        SlotClear(ConsumpContent);
        SlotClear(OtherContent);

        List<InventoryData> inventoryDataList = InventoryManager.Instance.GetDataFromDatabase();        

        foreach (InventoryData item in inventoryDataList)
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
    }
    private void SetItemToEquipSlot(Sprite sprite, int itemQuantity, int index)
    {                
        UI_InventorySlot slot = EquipContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Equipment, sprite, itemQuantity);
    }
    private void SetItemToConsumpSlot(Sprite sprite, int itemQuantity, int index)
    {        
        UI_InventorySlot slot = ConsumpContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Consump, sprite, itemQuantity);
    }
    private void SetItemToOtherSlot(Sprite sprite, int itemQuantity, int index)
    {        
        UI_InventorySlot slot = OtherContent.transform.GetChild(index).GetComponent<UI_InventorySlot>();        
        slot.Initialize(itemID, Item_Type.Other, sprite, itemQuantity);
    }
    private void SlotClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            UI_InventorySlot slot = content.transform.GetChild(i).GetComponent<UI_InventorySlot>();
            slot.SlotClear();
        }
    }
}
