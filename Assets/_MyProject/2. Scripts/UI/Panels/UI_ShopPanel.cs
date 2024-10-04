using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopPanel : MonoBehaviour
{
    /// <summary>
    /// 어떤 npc의 상점인지
    /// </summary>
    private int npc_ID;

    [Header("상점 아이템 오브젝트")]
    /// <summary>
    /// 상점 아이템들을 진열할 곳
    /// </summary>
    public GameObject shopItemListContent;   
    public GameObject shopItemPrefab;        
    [Header("잔액 부족 경고")]
    public GameObject Warining_NOMoney;
    [Header("상점 구매")]
    public GameObject BuyWindow;
    [Header("상점 판매")]
    public GameObject SellWindow;
    public List<Button> itemTabButtonList;          // 장비, 소비, 기타 탭 버튼
    public List<GameObject> itemContentList;        // 장비, 소비, 기타 아이템 리스트가 들어갈 곳
    public UI_ShopSellItemInfo sellItemInfo;
    private GameObject EquipContent;
    private GameObject ConsumpContent;
    private GameObject OtherContent;

    [Header("구매 / 판매 수량 UI")]
    public UI_BuyAmount UI_BuyAmount;
    public UI_SellAmount uI_SellAmount;    
    [Header("상점 나가기 버튼")]
    public Button ShopExitButton;
    [Header("소지 골드")]
    public TMP_Text goldText;
    [Header("플레이어 UI")]
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
    
    #region 구매
    private void SetShopItemList()
    {
        shopItemListContent.ContentClear();
        // 상점의 아이템 리스트 가져오기
        List<NPC_Shop_Item_Data> shopItemList = ShopManager.Instance.GetShopItemList(this.npc_ID);
        Debug.Log($"몇 개? : {shopItemList.Count}");
        // 아이템 진열하기
        foreach (var shopItem in shopItemList)
        {
            UI_ShopItemBuyPrefab ui_ShopItem = Instantiate(shopItemPrefab, shopItemListContent.transform).GetComponent<UI_ShopItemBuyPrefab>();
            ui_ShopItem.SetBuyShopItem(transform.root.GetComponent<PlayerController>().userID, shopItem);
        }
    }
    /// <summary>
    /// 아이템 사거나 파는 것을 성공했을 때 플레이어 소지금 변경
    /// </summary>
    public void OnSuccessBuySell()
    {
        SetPlayerGold();
    }
    /// <summary>
    /// 아이템 사는 것을 실패했을 때의 콜백함수
    /// </summary>
    public void OnFailureBuy()
    {
        Debug.Log("아이템 사는 것 실패");
        Warining_NOMoney.SetActive(false);
        Warining_NOMoney.SetActive(true);
    }
    #endregion

    #region 판매

    /// <summary>
    /// 판매를 위한 플레이어의 인벤토리 아이템 세팅
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
