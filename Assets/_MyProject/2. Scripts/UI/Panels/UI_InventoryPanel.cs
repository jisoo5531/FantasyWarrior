using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPanel : MonoBehaviour
{    
    // TODO : 0910. 현재 장비 아이템만 커서 올릴 때 아이템 정보 나옴. 소비 아이템 또는 기타 아이템 정보 UI도 만들어서 관리
    // 아이템 종류 더 많게 해서 테스트 해보기.    
    // TODO : 인벤토리 정렬 기능 넣기
    

    [Header("Equip")]
    public Button EquipTabButton;
    public GameObject EquipContent;

    [Header("Consump")]
    public Button ConsumpTabButton;
    public GameObject ConsumpContent;

    [Header("Other")]
    public Button OtherTabButton;
    public GameObject OtherContent;

    [Header("Item Info")]
    public UI_EquipItemInfo EquipitemInfo;
    public UI_ConsumpOtherItemInfo OtherConsumpItemInfo;

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
        //PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot_Sort;
        //PlayerEquipManager.Instance.OnUnEquipItem += SetItemToSlot_Sort;
        //InventoryManager.Instance.OnGetItem += SetItemToSlot_Sort;

        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        PlayerEquipManager.Instance.OnUnEquipItem += SetItemToSlot;
        InventoryManager.Instance.OnGetItem += SetItemToSlot;

        List<InventoryData> inventoryDataList = InventoryManager.Instance.GetDataFromDatabase();
        if (inventoryDataList.Count > 0)
        {
            Debug.Log("왜 안돼?");
            SetItemToSlot_Sort();
            //SetItemToSlot();
        }        
    }
    /// <summary>
    /// <para>인벤토리 슬롯으로 세팅</para>
    /// 인벤토리에 빈 곳 먼저 세팅.
    /// </summary>
    private void SetItemToSlot()
    {
        Queue<UI_InventorySlot> emptyEquipSlot = EmptySlotCheck(EquipContent);   
        Queue<UI_InventorySlot> emptyConsumpSlot = EmptySlotCheck(ConsumpContent);   
        Queue<UI_InventorySlot> emptyOtherSlot = EmptySlotCheck(OtherContent);

        Debug.Log(InventoryManager.Instance.addWhichItemList.Count);
        
        List<InventoryData> inventoryDataList = InventoryManager.Instance.GetDataFromDatabase();        
        
        foreach (int itemID in InventoryManager.Instance.addWhichItemList)
        {
            this.itemID = itemID;
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);
            Item_Type itemType = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            int itemQuantity = InventoryManager.Instance.GetItemQuantity(itemID);

            switch (itemType)
            {
                case Item_Type.Equipment:
                    SetItemToEquipSlot(sprite, itemQuantity, emptyEquipSlot.Dequeue());
                    break;
                case Item_Type.Consump:
                    SetItemToConsumpSlot(sprite, itemQuantity, emptyConsumpSlot.Dequeue());
                    break;
                case Item_Type.Other:
                    SetItemToOtherSlot(sprite, itemQuantity, emptyOtherSlot.Dequeue());
                    break;
                default:
                    break;
            }
        }

        // 다 더하고 난 뒤에 초기화.
        InventoryManager.Instance.ClearAddWhichItemList();
    }
    private Queue<UI_InventorySlot> EmptySlotCheck(GameObject content)
    {
        Queue<UI_InventorySlot> emptySlot = new Queue<UI_InventorySlot>();

        for (int i = 0; i < content.transform.childCount; i++)
        {
            UI_InventorySlot tmpSlot = content.transform.GetChild(i).GetComponent<UI_InventorySlot>();
            if (tmpSlot.itemID == 0)
            {
                emptySlot.Enqueue(tmpSlot);
            }
        }
        return emptySlot;
    }
    /// <summary>
    /// <para>인벤토리 테이블에 저장된 아이템들을 인벤토리 슬롯으로 세팅</para>
    /// <para>아이템 획득, 장비 장착, 장비 해제 등등. 인벤토리가 업데이트되면 실행.</para>
    /// 정렬(앞으로 밀기) 하여 인벤토리에 세팅 가능.
    /// </summary>
    private void SetItemToSlot_Sort()
    {
        int equipAmount = 0;
        int consumpAmount = 0;
        int otherAmount = 0;
        SlotClear(EquipContent);
        SlotClear(ConsumpContent);
        SlotClear(OtherContent);

        List<InventoryData> inventoryDataList = InventoryManager.Instance.GetDataFromDatabase();

        Debug.Log(inventoryDataList.Count);
        foreach (InventoryData item in inventoryDataList)
        {            
            this.itemID = item.Item_ID;
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);
            Item_Type itemType = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            int itemQuantity = item.Quantity;

            UI_InventorySlot slot = null;
            switch (itemType)
            {
                case Item_Type.Equipment:
                    slot = EquipContent.transform.GetChild(equipAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToEquipSlot(sprite, itemQuantity, slot);
                    break;
                case Item_Type.Consump:
                    slot = ConsumpContent.transform.GetChild(consumpAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToConsumpSlot(sprite, itemQuantity, slot);
                    break;
                case Item_Type.Other:
                    slot = OtherContent.transform.GetChild(otherAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToOtherSlot(sprite, itemQuantity, slot);
                    break;
                default:
                    break;
            }
        }
    }
    private void SetItemToEquipSlot(Sprite sprite, int itemQuantity, UI_InventorySlot slot)
    {                        
        slot.Initialize(itemID, sprite, EquipitemInfo);
    }
    private void SetItemToConsumpSlot(Sprite sprite, int itemQuantity, UI_InventorySlot slot)
    {                     
        slot.Initialize(itemID, sprite, OtherConsumpItemInfo);
    }
    private void SetItemToOtherSlot(Sprite sprite, int itemQuantity, UI_InventorySlot slot)
    {                        
        slot.Initialize(itemID, sprite, OtherConsumpItemInfo);
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
