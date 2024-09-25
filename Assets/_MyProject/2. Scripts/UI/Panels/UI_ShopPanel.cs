using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("상점 나가기 버튼")]
    public Button ShopExitButton;
    [Header("플레이어 UI")]
    public GameObject PlayerUI;

    private void Awake()
    {
        ShopExitButton.onClick.AddListener(OnClickExitShopButton);
    }

    private void Start()
    {
        
    }

    public void Initialize(int npcID)
    {
        this.npc_ID = npcID;

        SetShopItemList();
    }   
    private void SetShopItemList()
    {
        // 상점의 아이템 리스트 가져오기
        List<NPC_Shop_Item_Data> shopItemList = ShopManager.Instance.GetShopItemList(this.npc_ID);
        Debug.Log($"몇 개? : {shopItemList.Count}");
        // 아이템 진열하기
        foreach (var shopItem in shopItemList)
        {
            UI_ShopItemPrefab ui_ShopItem = Instantiate(shopItemPrefab, shopItemListContent.transform).GetComponent<UI_ShopItemPrefab>();
            ui_ShopItem.Initialize(ItemManager.Instance.GetItemData(shopItem.Item_ID));
        }
    }
    private void OnClickExitShopButton()
    {
        gameObject.SetActive(false);
        PlayerUI.SetActive(true);
    }
}
