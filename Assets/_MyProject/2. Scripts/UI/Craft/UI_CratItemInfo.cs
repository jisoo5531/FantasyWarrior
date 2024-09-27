using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI_CratItemInfo : MonoBehaviour
{
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

    private List<C_MaterialPossesion> m_Possesion;

    private void Awake()
    {
        quantityInputText.onValueChanged.AddListener(OnValueChangedInputQuantity);
        ButtonInitialize();
    }
    private void ButtonInitialize()
    {
        Q_UpButton.onClick.AddListener(OnClickAmountUpButton);
        Q_DownButton.onClick.AddListener(OnClickAmountDownButton);
        backButton.onClick.AddListener(OnClickBackButton);
    }
    public void Initialize(int npcID, ItemData item)
    {
        m_Possesion = new List<C_MaterialPossesion>();
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
    /// 만들어질 수량 체크를 할 때 사용할 클래스에 데이터 추가
    /// </summary>
    /// <param name="reqAmount"></param>
    /// <param name="haveAmount"></param>
    private void AddMaterialPossesion(int reqAmount, int haveAmount)
    {
        Debug.Log($"{reqAmount}, {haveAmount}");
        m_Possesion.Add(new C_MaterialPossesion(reqAmount, haveAmount));
    }
    private bool CheckPossesion()
    {
        foreach (var materialPossesion in m_Possesion)
        {
            if (false == materialPossesion.CheckAmount(itemQuantity))
            {
                return false;                
            }            
        }
        return true;
    }
    /// <summary>
    /// 만들 수 있는 최대 수량 가져오기
    /// </summary>
    private void MaxCraftQuantity()
    {
        List<int> maxAmountList = new List<int>();
        foreach (var materialPossesion in m_Possesion)
        {            
            maxAmountList.Add(materialPossesion.MaxAmount());
        }
        foreach (var item in maxAmountList)
        {
            Debug.Log(item);
        }
        
        maxCraftText.text = maxAmountList.Min().ToString();
    }
    /// <summary>
    /// 수량 Up 버튼 클릭
    /// </summary>
    private void OnClickAmountUpButton()
    {
        itemQuantity += 1;
        if (false == CheckPossesion())
        {
            // 만약 재료가 그만큼 없으면 
            itemQuantity -= 1;
        }
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
        // 만들 수량만큼 재료가 없으면 
    }
    private void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
