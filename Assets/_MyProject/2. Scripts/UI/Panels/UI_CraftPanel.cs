using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CraftPanel : MonoBehaviour
{
    /// <summary>
    /// � npc�� �����ϴ���
    /// </summary>
    private int npc_ID;

    [Header("������ ����")]
    public GameObject recipeContent;            // ������ ������ ����Ʈ�� �� ��
    public UI_CraftItemPrefab uI_CraftItemPrefab;

    [Header("���� ������ ������")]
    public GameObject craftItemInfoWindow;

    [Header("������ ��ư")]
    public Button exitButton;

    [Header("�÷��̾� UI")]
    public GameObject playerUIOBj;
    [Header("���� ���� �޽���")]
    public GameObject error_CraftWindow;

    [Header("�÷��̾� ���� ���")]
    public TMP_Text playerGoldText;

    public void Initialize(int npcID)
    {
        this.npc_ID = npcID;
        ButtonInitialize();
        SetCraftItemList();
        craftItemInfoWindow.SetActive(false);
        SetPlayerGold();
    }    
    private void ButtonInitialize()
    {
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    /// <summary>
    /// ���� npc�� ������ �ִ� ������ ����ŭ content�� ����
    /// </summary>
    private void SetCraftItemList()
    {
        recipeContent.ContentClear();
        foreach (var recipe in BlacksmithManager.Instance.GetBlacksmithRecipeList(this.npc_ID))
        {
            UI_CraftItemPrefab craftItemObj = Instantiate(uI_CraftItemPrefab, recipeContent.transform).GetComponent<UI_CraftItemPrefab>();
            craftItemObj.SetCraftItemRecipe(this.npc_ID, recipe);
        }        
    }
    public void SetPlayerGold()
    {
        playerGoldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
        playerUIOBj.SetActive(true);
    }
}
