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

    [Header("������ �̹���")]
    public Image itemIcon;
    [Header("���")]
    public TMP_Text playerGoldText;
    public TMP_Text craftitemCostText;    
    [Header("���� Input")]
    public TMP_InputField quantityInputText;
    [Header("��ư")]
    public Button backButton;
    public Button Q_UpButton;
    public Button Q_DownButton;
    public Button craftButton;
    [Header("���� ��� ����Ʈ")]
    public GameObject craftMaterialContent;
    public GameObject craftMaterialPrefab;
    [Header("���� �� �ִ� �ִ� ����")]
    public TMP_Text maxCraftText;

    /// <summary>
    /// ������� ��� �������� ������
    /// </summary>
    private ItemData craftItem;
    /// <summary>
    /// �ش� �������� ������ ����
    /// </summary>
    private CraftingRecipeData recipeData;
    /// <summary>
    /// ���� ����
    /// </summary>
    private int itemQuantity;
    /// <summary>
    /// ��������� �� ���� ���
    /// </summary>
    private int craftCost;
    /// <summary>
    /// ���� �� �ִ� �ִ� ����
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
    /// UI�� �����ϱ�
    /// </summary>
    private void SetDisplay()
    {
        itemQuantity = 0;
        quantityInputText.text = "0";
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{craftItem.Item_Name}");
        playerGoldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }
    /// <summary>
    /// ���ۿ� �ʿ��� ��� ������ ����Ʈ�� UI�� �����ϱ�
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
    /// �κ��丮���� �������� ������ ���� �÷��̾ ������ ��ῡ���� ������    
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
    /// ������� ���� üũ�� �� �� ����� Ŭ������ ������ �߰�
    /// </summary>
    /// <param name="reqAmount"></param>
    /// <param name="haveAmount"></param>
    private void AddMaterialPossesion(int itemId, int reqAmount, int haveAmount)
    {
        Debug.Log($"{reqAmount}, {haveAmount}");
        m_Possesion_Dict.Add(itemId, new C_MaterialPossesion(itemId, reqAmount, haveAmount));        
    }
    /// <summary>
    /// ���� �� �ִ� �ִ� ���� ��������
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
    /// ���� Up ��ư Ŭ��
    /// </summary>
    private void OnClickAmountUpButton()
    {
        itemQuantity += 1;

        if (itemQuantity > maxCraft)
        {
            // ���� ��ᰡ �׸�ŭ ������ 
            itemQuantity = maxCraft;
        }
        UpdateItemCostGoldText();
        UpdateQuantityText();
    }
    /// <summary>
    /// ���� Down ��ư Ŭ��
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
    /// ������ ������ input���� �Է��Ͽ� ���� ����� �� ȣ��
    /// </summary>
    /// <param name="value"></param>
    private void OnValueChangedInputQuantity(string value)
    {
        itemQuantity = int.Parse(value);
        if (itemQuantity > maxCraft)
        {
            // ���� ������ŭ ��ᰡ ������ 
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
