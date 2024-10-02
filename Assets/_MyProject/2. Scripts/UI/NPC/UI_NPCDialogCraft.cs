using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCDialogCraft : UI_NPCDialogShop
{
    [Header("장비 제작 대화")]
    public GameObject dialogCraftPrefab;
    [Header("장비 제작창")]
    public UI_CraftPanel craftPanel;
    protected override void SelectDialogInit()
    {
        base.SelectDialogInit();
        craftPanel = FindObjectOfType<UI_CraftPanel>(true);
        Instantiate(dialogCraftPrefab, DialogSelectContent.transform).GetComponent<Button>().onClick.AddListener(OnClickCraftDialog);
    }

    /// <summary>
    /// 대화 선택창에서 제작을 누르면
    /// </summary>
    private void OnClickCraftDialog()
    {
        
        craftPanel.Initialize(this.NPC_ID);
        craftPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        playerUI.gameObject.SetActive(false);
    }
}
