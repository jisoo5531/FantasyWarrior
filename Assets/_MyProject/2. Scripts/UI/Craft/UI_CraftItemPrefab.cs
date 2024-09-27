using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� ǰ�� UI ������
/// </summary>
public class UI_CraftItemPrefab : UI_ShopOrCraftItemPrefab
{
    private int npc_ID;
    private BlacksmithRecipeData RecipeData;
    private CraftingRecipeData CraftingRecipe;

    /// <summary>
    /// ������ ���� ������ �׸� ����
    /// </summary>
    public void SetCraftItemRecipe(int npcID, BlacksmithRecipeData recipeData)
    {
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
        UI_CratItemInfo craftItemInfo = PanelManager.Instance.craftPanel.craftItemInfoWindow.GetComponent<UI_CratItemInfo>();
        craftItemInfo.Initialize(this.npc_ID, ItemManager.Instance.GetItemData(CraftingRecipe.CraftItem_ID));
        PanelManager.Instance.craftPanel.craftItemInfoWindow.SetActive(true);
    }
}
