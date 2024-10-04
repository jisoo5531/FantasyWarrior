using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI_CratItemInfo : MonoBehaviour
{
    private int userId;
    private int npc_ID;

    [Header("아이템 이미지")]
    public Image itemIcon;
    [Header("골드")]
    public TMP_Text playerGoldText;
    public TMP_Text craftitemCostText;    
    [Header("수량 Input")]
    public TMP_InputField quantityInputText;
    [Header("버튼")]
    public Button backButton;
    public Button Q_UpButton;
    public Button Q_DownButton;
    public Button craftButton;
    [Header("제작 재료 리스트")]
    public GameObject craftMaterialContent;
    public GameObject craftMaterialPrefab;
    [Header("만들 수 있는 최대 수량")]
    public TMP_Text maxCraftText;

    /// <summary>
    /// 만들어질 대상 아이템의 데이터
    /// </summary>
    private ItemData craftItem;
    /// <summary>
    /// 해당 아이템의 레시피 정보
    /// </summary>
    private CraftingRecipeData recipeData;
    /// <summary>
    /// 만들 수량
    /// </summary>
    private int itemQuantity;
    /// <summary>
    /// 만들어지는 데 들어가는 비용
    /// </summary>
    private int craftCost;
    /// <summary>
    /// 만들 수 있는 최대 수량
    /// </summary>
    private int maxCraft;

    private Dictionary<int, C_MaterialPossesion> m_Possesion_Dict;

    private void Awake()
    {
        quantityInputText.onValueChanged.AddListener(OnValueChangedInputQuantity);
        InventoryManager.Instance.OnSubtractItem += SubtractMatriealItem;
        ButtonInitialize();
    }
    
    private void ButtonInitialize()
    {
        Q_UpButton.onClick.AddListener(OnClickAmountUpButton);
        Q_DownButton.onClick.AddListener(OnClickAmountDownButton);
        backButton.onClick.AddListener(OnClickBackButton);
        craftButton.onClick.AddListener(OnClickCraftButton);
    }
    public void Initialize(int userId, int npcID, ItemData item)
    {
        m_Possesion_Dict = new Dictionary<int, C_MaterialPossesion>();
        this.userId = userId;
        this.npc_ID = npcID;
        this.craftItem = item;
        this.recipeData = CraftRecipeManager.Instance.GetRecipeData(item_ID: craftItem.Item_ID);
        this.craftCost = BlacksmithManager.Instance.GetCraftItemCost(npc_ID, this.recipeData.Recipe_ID);
        SetDisplay();
        SetCraftMaterialsList();
    }
    /// <summary>
    /// UI에 진열하기
    /// </summary>
    private void SetDisplay()
    {
        itemQuantity = 0;
        quantityInputText.text = "0";
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{craftItem.Item_Name}");
        playerGoldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }
    /// <summary>
    /// 제작에 필요한 재료 아이템 리스트들 UI에 나열하기
    /// </summary>
    private void SetCraftMaterialsList()
    {
        craftMaterialContent.ContentClear();
        foreach (var recipeMaterial in CraftRecipeManager.Instance.GetRecipeMaterialList(this.recipeData.Recipe_ID))
        {            
            UI_CraftMaterialInfo ui_MaterialInfo = Instantiate(craftMaterialPrefab, craftMaterialContent.transform).GetComponent<UI_CraftMaterialInfo>();
            ui_MaterialInfo.Initialize(recipeMaterial, AddMaterialPossesion);            
        }
        MaxCraftQuantity();
    }
    /// <summary>
    /// 인벤토리에서 아이템이 빠지면 현재 플레이어가 보유한 재료에서도 빠지게    
    /// </summary>
    /// <param name="item"></param>
    private void SubtractMatriealItem(ItemData item)
    {
        if (m_Possesion_Dict.TryGetValue(item.Item_ID, out C_MaterialPossesion matrieal))
        {
            matrieal.haveAmount = InventoryManager.Instance.GetInventoryItem(item.Item_ID).Quantity;
        }
        MaxCraftQuantity();
    }
    /// <summary>
    /// 만들어질 수량 체크를 할 때 사용할 클래스에 데이터 추가
    /// </summary>
    /// <param name="reqAmount"></param>
    /// <param name="haveAmount"></param>
    private void AddMaterialPossesion(int itemId, int reqAmount, int haveAmount)
    {
        Debug.Log($"{reqAmount}, {haveAmount}");
        m_Possesion_Dict.Add(itemId, new C_MaterialPossesion(itemId, reqAmount, haveAmount));        
    }
    /// <summary>
    /// 만들 수 있는 최대 수량 가져오기
    /// </summary>
    private void MaxCraftQuantity()
    {
        List<int> maxAmountList = new List<int>();
        foreach (var materialPossesion in m_Possesion_Dict)
        {            
            maxAmountList.Add(materialPossesion.Value.MaxAmount());
        }
        if (maxAmountList == null || maxAmountList.Count == 0)
        {
            maxCraftText.text = "0";
            return;
        }
        foreach (var item in maxAmountList)
        {
            Debug.Log(item);
        }
        maxCraft = maxAmountList.Min();
        maxCraftText.text = maxCraft.ToString();
    }
    /// <summary>
    /// 수량 Up 버튼 클릭
    /// </summary>
    private void OnClickAmountUpButton()
    {
        itemQuantity += 1;

        if (itemQuantity > maxCraft)
        {
            // 만약 재료가 그만큼 없으면 
            itemQuantity = maxCraft;
        }
        UpdateItemCostGoldText();
        UpdateQuantityText();
    }
    /// <summary>
    /// 수량 Down 버튼 클릭
    /// </summary>
    private void OnClickAmountDownButton()
    {
        itemQuantity -= 1;
        if (itemQuantity < 0)
        {
            itemQuantity = 0;
        }
        UpdateItemCostGoldText();
        UpdateQuantityText();
    }
    private void UpdateItemCostGoldText()
    {        
        craftitemCostText.text = (craftCost * itemQuantity).ToString();
    }
    private void UpdateQuantityText()
    {
        quantityInputText.text = itemQuantity.ToString();
    }
    /// <summary>
    /// 아이템 수량을 input으로 입력하여 값이 변경될 때 호출
    /// </summary>
    /// <param name="value"></param>
    private void OnValueChangedInputQuantity(string value)
    {
        itemQuantity = int.Parse(value);
        if (itemQuantity > maxCraft)
        {
            // 만들 수량만큼 재료가 없으면 
            itemQuantity = maxCraft;
        }
        UpdateItemCostGoldText();
        UpdateQuantityText();
    }
    private void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
    private void OnClickCraftButton()
    {
        if (craftCost * itemQuantity > UserStatManager.Instance.userStatClient.Gold)
        {
            FailureCraft();
            return;
        }
        CraftManager.Instance.CraftItem(userId, craftItem, itemQuantity, SuccessCraft);
        
    }
    private void SuccessCraft()
    {
        UserStatManager.Instance.UseGold(craftCost * itemQuantity);

        FindObjectOfType<PanelManager>(true).CraftPanel.SetPlayerGold();
        foreach (var recipeMaterial in CraftRecipeManager.Instance.GetRecipeMaterialList(this.recipeData.Recipe_ID))
        {
            ItemData recipeItem = ItemManager.Instance.GetItemData(recipeMaterial.M_Item_ID);
            InventoryManager.Instance.SubtractItem(recipeItem, recipeMaterial.M_Quantity * itemQuantity);
        }
        
        gameObject.SetActive(false);
    }
    private void FailureCraft()
    {
        FindObjectOfType<PanelManager>(true).CraftPanel.error_CraftWindow.SetActive(true);
    }
}
