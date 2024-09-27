using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void Initialize(int npcID)
    {
        this.npc_ID = npcID;
        ButtonInitialize();
        SetCraftItemList();
        craftItemInfoWindow.SetActive(false);
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

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
        playerUIOBj.SetActive(true);
    }
}
