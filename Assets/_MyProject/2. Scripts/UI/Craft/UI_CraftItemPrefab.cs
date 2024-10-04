using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� ǰ�� UI ������
/// </summary>
public class UI_CraftItemPrefab : UI_ShopOrCraftItemPrefab
{
    private int userID;
    private int npc_ID;
    private BlacksmithRecipeData RecipeData;
    private CraftingRecipeData CraftingRecipe;

    /// <summary>
    /// ������ ���� ������ �׸� ����
    /// </summary>
    public void SetCraftItemRecipe(int userID, int npcID, BlacksmithRecipeData recipeData)
    {
        this.userID = userID;
        this.npc_ID = npcID;
        this.RecipeData = recipeData;
        this.CraftingRecipe = CraftRecipeManager.Instance.GetRecipeData(recipe_ID: recipeData.Recipe_ID);
        Initialize(ItemManager.Instance.GetItemData(CraftingRecipe.CraftItem_ID));
    }

    /// <summary>
    /// ���� ������ Ŭ��
    /// </summary>
    protected override void OnClickItemInfoButton()
    {
        UI_CraftPanel craftPanel = FindObjectOfType<PanelManager>(true).CraftPanel;
        UI_CratItemInfo craftItemInfo = craftPanel.craftItemInfoWindow.GetComponent<UI_CratItemInfo>();
        craftItemInfo.Initialize(userID, this.npc_ID, ItemManager.Instance.GetItemData(CraftingRecipe.CraftItem_ID));
        craftPanel.craftItemInfoWindow.SetActive(true);
    }
}
