using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void Initialize(int npcID)
    {
        this.npc_ID = npcID;
        SetCraftItemList();
    }

    /// <summary>
    /// ���� npc�� ������ �ִ� ������ ����ŭ content�� ����
    /// </summary>
    private void SetCraftItemList()
    {
        foreach (var recipe in BlacksmithManager.Instance.GetBlacksmithRecipeList(this.npc_ID))
        {
            UI_CraftItemPrefab craftItemObj = Instantiate(uI_CraftItemPrefab, recipeContent.transform).GetComponent<UI_CraftItemPrefab>();
            craftItemObj.SetCraftItemRecipe(recipe);
        }        
    }
}
