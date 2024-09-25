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

    [Header("���� ���")]
    public TMP_Text goldText;
    [Header("���� ������ ��ư")]
    public Button ShopExitButton;
    [Header("�ܾ� ���� ���")]
    public GameObject Warining_NOMoney;
    [Header("�÷��̾� UI")]
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
        // ������ ������ ����Ʈ ��������
        List<NPC_Shop_Item_Data> shopItemList = ShopManager.Instance.GetShopItemList(this.npc_ID);
        Debug.Log($"�� ��? : {shopItemList.Count}");
        // ������ �����ϱ�
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
    /// ������ ��� ���� �������� ���� �ݹ��Լ�
    /// </summary>
    private void OnSuccessBuy()
    {
        SetPlayerGold();
    }
    /// <summary>
    /// ������ ��� ���� �������� ���� �ݹ��Լ�
    /// </summary>
    private void OnFailureBuy()
    {
        Debug.Log("������ ��� �� ����");
        Warining_NOMoney.SetActive(false);
        Warining_NOMoney.SetActive(true);
    }
}
