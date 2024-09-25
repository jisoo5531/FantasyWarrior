using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventoryPanel : MonoBehaviour
{
    // TODO : ������ ���� �� ���� �ؼ� �׽�Ʈ �غ���.    
    // TODO : �κ��丮 ���� ��� �ֱ�


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

    [Header("Gold")]    
    public TMP_Text goldText;

    private int itemID;
    /// <summary>
    /// � ���� �ٲ������ �񱳸� ���� �κ��丮 ����Ʈ
    /// </summary>
    private List<InventoryData> oldInventoryList = new List<InventoryData>();

    private Queue<UI_InventorySlot> emptyEquipSlot = new();
    private Queue<UI_InventorySlot> emptyConsumpSlot = new();
    private Queue<UI_InventorySlot> emptyOtherSlot = new();
    

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
        InventoryPanelInit();
    }
    public void InventoryPanelInit()
    {
        //PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot_Sort;
        //PlayerEquipManager.Instance.OnUnEquipItem += SetItemToSlot_Sort;
        //InventoryManager.Instance.OnGetItem += SetItemToSlot_Sort;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        PlayerEquipManager.Instance.OnUnEquipItem += SetItemToSlot;
        InventoryManager.Instance.OnGetItem += SetItemToSlot;
        InventoryManager.Instance.OnGetItem += SetPlayerGold;


        List<InventoryData> inventoryDataList = InventoryManager.Instance.inventoryDataList;
        if (inventoryDataList.Count > 0)
        {
            oldInventoryList = inventoryDataList.Select(item => new InventoryData(item.User_ID, item.Item_ID, item.Quantity)).ToList();

            Debug.Log("�� �ȵ�?");
            SetItemToSlot_Sort();
            //SetItemToSlot();
        }
        SetPlayerGold();
    }
    
    /// <summary>
    /// ������ ������ ��� ������ ����
    /// </summary>
    private void SetPlayerGold()
    {
        goldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }

    /// <summary>
    /// <para>�κ��丮 �������� ����</para>
    /// �κ��丮�� �� �� ���� ����.
    /// </summary>
    private void SetItemToSlot()
    {
        List<InventoryData> newInventoryList = InventoryManager.Instance.inventoryDataList;
        List<AddItemClassfiy> whichAddItem = InventoryManager.Instance.addWhichItemList;

        foreach (var item in whichAddItem)
        {
            if (item.isExist)
            {
                // ���� �������� �κ��丮�� �����ϸ�
                ModifiedItemSlotSetting(item.item_ID);
            }
            else
            {
                // �������� ������
                AddItemSlotSetting(item.item_ID);
            }
        }

        // �� ���ϰ� �� �ڿ� �ʱ�ȭ.        
        InventoryManager.Instance.ClearAddWhichItemList();
    }
    private void AddItemSlotSetting(int itemID)
    {
        emptyEquipSlot = EmptySlotCheck(EquipContent);
        emptyConsumpSlot = EmptySlotCheck(ConsumpContent);
        emptyOtherSlot = EmptySlotCheck(OtherContent);

        string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);
        Item_Type? itemType = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
        Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");

        switch (itemType)
        {
            case Item_Type.Equipment:
                SetItemToEquipSlot(itemID, sprite, emptyEquipSlot.Dequeue());
                break;
            case Item_Type.Consump:
                SetItemToConsumpSlot(itemID, sprite, emptyConsumpSlot.Dequeue());
                break;
            case Item_Type.Other:
                SetItemToOtherSlot(itemID, sprite, emptyOtherSlot.Dequeue());
                break;
            default:
                break;
        }
    }
    private void ModifiedItemSlotSetting(int itemID)
    {
        List<InventoryData> inventoryDataList = InventoryManager.Instance.inventoryDataList;
        int index = inventoryDataList.FindIndex(x => x.Item_ID.Equals(itemID));
        int quantity = inventoryDataList[index].Quantity;
        for (int i = 0; i < EquipContent.transform.childCount; i++)
        {            
            UI_InventorySlot slot = EquipContent.transform.GetChild(i).GetComponent<UI_InventorySlot>();
            if (slot.itemID.Equals(itemID))
            {
                slot.itemQuantityText.text = quantity.ToString();
                break;
            }
        }
        for (int i = 0; i < ConsumpContent.transform.childCount; i++)
        {
            UI_InventorySlot slot = ConsumpContent.transform.GetChild(i).GetComponent<UI_InventorySlot>();
            if (slot.itemID.Equals(itemID))
            {
                slot.itemQuantityText.text = quantity.ToString();
                break;
            }
        }
        for (int i = 0; i < OtherContent.transform.childCount; i++)
        {
            UI_InventorySlot slot = OtherContent.transform.GetChild(i).GetComponent<UI_InventorySlot>();
            if (slot.itemID.Equals(itemID))
            {
                slot.itemQuantityText.text = quantity.ToString();
                break;
            }
        }

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
    /// <para>�κ��丮 ���̺� ����� �����۵��� �κ��丮 �������� ����</para>
    /// <para>������ ȹ��, ��� ����, ��� ���� ���. �κ��丮�� ������Ʈ�Ǹ� ����.</para>
    /// ����(������ �б�) �Ͽ� �κ��丮�� ���� ����.
    /// </summary>
    private void SetItemToSlot_Sort()
    {
        int equipAmount = 0;
        int consumpAmount = 0;
        int otherAmount = 0;
        SlotClear(EquipContent);
        SlotClear(ConsumpContent);
        SlotClear(OtherContent);

        List<InventoryData> inventoryDataList = InventoryManager.Instance.inventoryDataList;

        foreach (InventoryData item in inventoryDataList)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(item.Item_ID);
            Item_Type? itemType = ItemManager.Instance.GetInventoryItemTypeFromDB(item.Item_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            int itemQuantity = item.Quantity;

            UI_InventorySlot slot = null;
            switch (itemType)
            {
                case Item_Type.Equipment:
                    slot = EquipContent.transform.GetChild(equipAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToEquipSlot(item.Item_ID, sprite, slot);
                    break;
                case Item_Type.Consump:
                    slot = ConsumpContent.transform.GetChild(consumpAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToConsumpSlot(item.Item_ID, sprite, slot);
                    break;
                case Item_Type.Other:
                    slot = OtherContent.transform.GetChild(otherAmount++).GetComponent<UI_InventorySlot>();
                    SetItemToOtherSlot(item.Item_ID, sprite, slot);
                    break;
                default:
                    break;
            }
        }
    }
    private void SetItemToEquipSlot(int itemID, Sprite sprite, UI_InventorySlot slot)
    {
        slot.Initialize(itemID, sprite, EquipitemInfo);
    }
    private void SetItemToConsumpSlot(int itemID, Sprite sprite, UI_InventorySlot slot)
    {
        slot.Initialize(itemID, sprite, OtherConsumpItemInfo);
    }
    private void SetItemToOtherSlot(int itemID, Sprite sprite, UI_InventorySlot slot)
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
