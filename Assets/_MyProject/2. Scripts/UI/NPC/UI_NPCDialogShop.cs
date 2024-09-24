using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCDialogShop : UI_NPCDialogue
{
    [Header("���� ��ȭ")]
    public GameObject dialogShopBuyPrefab;
    public GameObject dialogShopSellPrefab;

    protected override void SelectDialogInit()
    {
        base.SelectDialogInit();
        Instantiate(dialogShopBuyPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickBuyButton);
        Instantiate(dialogShopSellPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickSellButton);
    }

    /// <summary>
    /// �������� ���� ��� ��ư�� Ŭ���ϸ�
    /// </summary>
    private void OnClickBuyButton()
    {
        Debug.Log("���� ����");
    }
    /// <summary>
    /// �������� ���� �ȱ� ��ư�� Ŭ���ϸ�
    /// </summary>
    private void OnClickSellButton()
    {
        Debug.Log("���� ����");
    }
}
