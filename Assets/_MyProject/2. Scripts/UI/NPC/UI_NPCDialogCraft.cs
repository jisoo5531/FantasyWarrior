using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCDialogCraft : UI_NPCDialogShop
{
    [Header("��� ���� ��ȭ")]
    public GameObject dialogCraftPrefab;
    [Header("��� ����â")]
    public UI_CraftPanel craftPanel;
    protected override void SelectDialogInit()
    {
        base.SelectDialogInit();
        craftPanel = FindObjectOfType<UI_CraftPanel>(true);
        Instantiate(dialogCraftPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickCraftDialog);
    }

    /// <summary>
    /// ��ȭ ����â���� ������ ������
    /// </summary>
    private void OnClickCraftDialog()
    {
        
        craftPanel.Initialize(this.NPC_ID);
        craftPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        playerUI.gameObject.SetActive(false);
    }
}
