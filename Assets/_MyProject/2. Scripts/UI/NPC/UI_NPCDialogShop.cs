using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCDialogShop : UI_NPCDialogue
{
    [Header("���� ��ȭ")]
    public GameObject dialogShopBuyPrefab;
    public GameObject dialogShopSellPrefab;

    [Header("����â")]
    public UI_ShopPanel shopPanel;

    protected override void SelectDialogInit()
    {
        base.SelectDialogInit();
        shopPanel = FindObjectOfType<UI_ShopPanel>(true);
        Instantiate(dialogShopBuyPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickBuyButton);
        Instantiate(dialogShopSellPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickSellButton);
    }

    /// <summary>
    /// �������� ���� ��� ��ư�� Ŭ���ϸ�
    /// </summary>
    private void OnClickBuyButton()
    {
        Debug.Log("���� ����");
        
        shopPanel.Initialize(this.NPC_ID, ShopDL_Type.Buy);
        shopPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        playerUI.gameObject.SetActive(false);
    }
    /// <summary>
    /// �������� ���� �ȱ� ��ư�� Ŭ���ϸ�
    /// </summary>
    private void OnClickSellButton()
    {
        Debug.Log("���� ����");        
        shopPanel.Initialize(this.NPC_ID, ShopDL_Type.Sell);
        shopPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        playerUI.gameObject.SetActive(false);
    }
}
public enum ShopDL_Type
{
    Buy,
    Sell
}