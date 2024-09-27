using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPanel : MonoBehaviour
{
    /// <summary>
    /// 어떤 npc가 제작하는지
    /// </summary>
    private int npc_ID;

    [Header("아이템 제작")]
    public GameObject recipeContent;            // 제작할 아이템 리스트가 들어갈 곳
    public UI_CraftItemPrefab uI_CraftItemPrefab;

    [Header("제작 아이템 상세정보")]
    public GameObject craftItemInfoWindow;

    [Header("나가기 버튼")]
    public Button exitButton;

    [Header("플레이어 UI")]
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
    /// 제작 npc가 가지고 있는 레시피 수만큼 content에 세팅
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
