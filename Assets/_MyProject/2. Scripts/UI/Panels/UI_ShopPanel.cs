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

    [Header("소지 골드")]
    public TMP_Text goldText;
    [Header("상점 나가기 버튼")]
    public Button ShopExitButton;
    [Header("잔액 부족 경고")]
    public GameObject Warining_NOMoney;
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
        SetPlayerGold();
        SetShopItemList();
    }
    private void SetPlayerGold()
    {
        goldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }
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
            ui_ShopItem.Initialize(shopItem, OnSuccessBuy, OnFailureBuy);
        }
    }
    private void OnClickExitShopButton()
    {
        gameObject.SetActive(false);
        PlayerUI.SetActive(true);
    }
    /// <summary>
    /// 아이템 사는 것을 성공했을 때의 콜백함수
    /// </summary>
    private void OnSuccessBuy()
    {
        SetPlayerGold();
    }
    /// <summary>
    /// 아이템 사는 것을 실패했을 때의 콜백함수
    /// </summary>
    private void OnFailureBuy()
    {
        Debug.Log("아이템 사는 것 실패");
        Warining_NOMoney.SetActive(false);
        Warining_NOMoney.SetActive(true);
    }
}
