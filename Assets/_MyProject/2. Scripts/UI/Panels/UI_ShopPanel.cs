using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopPanel : MonoBehaviour
{
    /// <summary>
    /// � npc�� ��������
    /// </summary>
    private int npc_ID;

    [Header("���� ������ ������Ʈ")]
    /// <summary>
    /// ���� �����۵��� ������ ��
    /// </summary>
    public GameObject shopItemListContent;   
    public GameObject shopItemPrefab;        
    [Header("�ܾ� ���� ���")]
    public GameObject Warining_NOMoney;
    [Header("���� ����")]
    public GameObject BuyWindow;
    [Header("���� �Ǹ�")]
    public GameObject SellWindow;
    public List<Button> itemTabButtonList;          // ���, �Һ�, ��Ÿ �� ��ư
    public List<GameObject> itemContentList;        // ���, �Һ�, ��Ÿ ������ ����Ʈ�� �� ��
    public UI_ShopSellItemInfo sellItemInfo;
    private GameObject EquipContent;
    private GameObject ConsumpContent;
    private GameObject OtherContent;

    [Header("���� / �Ǹ� ���� UI")]
    public UI_BuyAmount UI_BuyAmount;
    public UI_SellAmount uI_SellAmount;    
    [Header("���� ������ ��ư")]
    public Button ShopExitButton;
    [Header("���� ���")]
    public TMP_Text goldText;
    [Header("�÷��̾� UI")]
    public GameObject PlayerUI;

    private void Start()
    {
        
    }
    private void ButtonInitialize()
    {
        this.EquipContent = itemContentList[0];
        this.ConsumpContent = itemContentList[1];
        this.OtherContent = itemContentList[2];
        ShopExitButton.onClick.AddListener(OnClickExitShopButton);
        
        for (int i = 0; i < itemTabButtonList.Count; i++)
        {
            int index = i;
            itemTabButtonList[i].onClick.AddListener(() => OnClickItemTabButton(index));
        }
    }    
    
    public void Initialize(int npcID, ShopDL_Type BuyOrSell)
    {
        this.npc_ID = npcID;
        SetPlayerGold();
        ButtonInitialize();

        switch (BuyOrSell)
        {
            case ShopDL_Type.Buy:
                SetShopItemList();
                BuyWindow.SetActive(true);
                SellWindow.SetActive(false);
                break;
            case ShopDL_Type.Sell:
                SetSellInventoryList();
                SellWindow.SetActive(true);
                BuyWindow.SetActive(false);
                break;
            default:
                break;
        }
        
    }    
    
    #region ����
    private void SetShopItemList()
    {
        shopItemListContent.ContentClear();
        // ������ ������ ����Ʈ ��������
        List<NPC_Shop_Item_Data> shopItemList = ShopManager.Instance.GetShopItemList(this.npc_ID);
        Debug.Log($"�� ��? : {shopItemList.Count}");
        // ������ �����ϱ�
        foreach (var shopItem in shopItemList)
        {
            UI_ShopItemBuyPrefab ui_ShopItem = Instantiate(shopItemPrefab, shopItemListContent.transform).GetComponent<UI_ShopItemBuyPrefab>();
            ui_ShopItem.SetBuyShopItem(transform.root.GetComponent<PlayerController>().userID, shopItem);
        }
    }
    /// <summary>
    /// ������ ��ų� �Ĵ� ���� �������� �� �÷��̾� ������ ����
    /// </summary>
    public void OnSuccessBuySell()
    {
        SetPlayerGold();
    }
    /// <summary>
    /// ������ ��� ���� �������� ���� �ݹ��Լ�
    /// </summary>
    public void OnFailureBuy()
    {
        Debug.Log("������ ��� �� ����");
        Warining_NOMoney.SetActive(false);
        Warining_NOMoney.SetActive(true);
    }
    #endregion

    #region �Ǹ�

    /// <summary>
    /// �ǸŸ� ���� �÷��̾��� �κ��丮 ������ ����
    /// </summary>
    private void SetSellInventoryList()
    {
        ContentClear();
        int equipAmount = 0;
        int consumpAmount = 0;
        int otherAmount = 0;
        List<InventoryData> inventoryDataList = InventoryManager.Instance.inventoryDataList;

        foreach (InventoryData invenItem in inventoryDataList)
        {
            ItemData item = ItemManager.Instance.GetItemData(invenItem.Item_ID);
            string itemName = item.Item_Name;
            Item_Type? itemType = item.Item_Type;
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");            

            UI_ShopSellItemSlot slot = null;
            switch (itemType)
            {
                case Item_Type.Equipment:
                    slot = EquipContent.transform.GetChild(equipAmount++).GetComponent<UI_ShopSellItemSlot>();
                    SetItemToSlot(invenItem, sprite, slot);
                    break;
                case Item_Type.Consump:
                    slot = ConsumpContent.transform.GetChild(consumpAmount++).GetComponent<UI_ShopSellItemSlot>();
                    SetItemToSlot(invenItem, sprite, slot);
                    break;
                case Item_Type.Other:
                    Debug.Log(OtherContent == null);
                    slot = OtherContent.transform.GetChild(otherAmount++).GetComponent<UI_ShopSellItemSlot>();
                    SetItemToSlot(invenItem, sprite, slot);
                    break;
                default:
                    break;
            }
        }
    }
    private void ContentClear()
    {
        foreach (var slot in itemContentList)
        {
            for (int i = 0; i < slot.transform.childCount; i++)
            {
                slot.transform.GetChild(i).GetComponent<UI_ShopSellItemSlot>().SlotClear();
            }
        }
    }
    private void SetItemToSlot(InventoryData item, Sprite sprite, UI_ShopSellItemSlot slot)
    {
        slot.Initialize(item, sprite);
    }
    private void OnClickItemTabButton(int num)
    {
        for (int i = 0; i < itemContentList.Count; i++)
        {
            itemContentList[i].transform.parent.parent.gameObject.SetActive(i == num);
        }
    }

    #endregion
    private void SetPlayerGold()
    {
        goldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }
    private void OnClickExitShopButton()
    {
        gameObject.SetActive(false);
        PlayerUI.SetActive(true);
    }    
    
}
