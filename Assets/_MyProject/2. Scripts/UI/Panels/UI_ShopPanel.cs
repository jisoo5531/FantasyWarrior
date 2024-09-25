using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("���� ������ ��ư")]
    public Button ShopExitButton;
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

        SetShopItemList();
    }   
    private void SetShopItemList()
    {
        // ������ ������ ����Ʈ ��������
        List<NPC_Shop_Item_Data> shopItemList = ShopManager.Instance.GetShopItemList(this.npc_ID);
        Debug.Log($"�� ��? : {shopItemList.Count}");
        // ������ �����ϱ�
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
