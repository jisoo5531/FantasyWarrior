using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCDialogShop : UI_NPCDialogue
{
    [Header("상점 대화")]
    public GameObject dialogShopBuyPrefab;
    public GameObject dialogShopSellPrefab;

    [Header("상점창")]
    public UI_ShopPanel shopPanel;

    protected override void SelectDialogInit()
    {
        base.SelectDialogInit();
        Instantiate(dialogShopBuyPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickBuyButton);
        Instantiate(dialogShopSellPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickSellButton);
    }

    /// <summary>
    /// 상점에서 물건 사기 버튼을 클릭하면
    /// </summary>
    private void OnClickBuyButton()
    {
        Debug.Log("물건 사자");
        shopPanel.Initialize(this.NPC_ID);
        shopPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        playerUI.SetActive(false);
    }
    /// <summary>
    /// 상점에서 물건 팔기 버튼을 클릭하면
    /// </summary>
    private void OnClickSellButton()
    {
        Debug.Log("물건 팔자");
    }
}
