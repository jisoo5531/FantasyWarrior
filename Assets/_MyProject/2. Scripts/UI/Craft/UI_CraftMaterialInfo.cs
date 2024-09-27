using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_CraftMaterialInfo : MonoBehaviour
{
    [Header("재료 정보")]
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text ReqAmountText;
    public TMP_Text playerHaveAmountText;
    

    private ItemData materialItem;
    private RecipeMaterialData recipeMaterial;

    private Action<int, int> postInit;

    public void Initialize(RecipeMaterialData recipeMaterial, Action<int, int> init)
    {
        this.recipeMaterial = recipeMaterial;
        this.postInit = init;
        this.materialItem = ItemManager.Instance.GetItemData(recipeMaterial.M_Item_ID);
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{materialItem.Item_Name}");
        TextInitialize();
    }
    private void TextInitialize()
    {
        itemNameText.text = materialItem.Item_Name;
        ReqAmountText.text = $"x{recipeMaterial.M_Quantity.ToString()}";

        int? haveAmount = InventoryManager.Instance.GetItemQuantity(materialItem.Item_ID);        
        if (haveAmount != null)
        {
            playerHaveAmountText.text = $"x{haveAmount.ToString()}";
            postInit?.Invoke(recipeMaterial.M_Quantity, haveAmount.Value);
        }        
    }
}
/// <summary>
/// 제작 수량을 체크하기 위한 클래스
/// </summary>
public class C_MaterialPossesion
{
    public int reqAmount { get; set; }
    public int haveAmount { get; set; }

    public C_MaterialPossesion(int reqAmount, int haveAmount)
    {
        this.reqAmount = reqAmount;
        this.haveAmount = haveAmount;
    }

    /// <summary>
    /// 만들 수 있는 최대 수량
    /// </summary>
    /// <returns></returns>
    public int MaxAmount()
    {        
        return haveAmount / reqAmount;
    }
    /// <summary>
    /// 수량만큼 만들 수 있는지 체크
    /// <para>매개변수는 만들 수량</para>
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public bool CheckAmount(int quantity)
    {
        return reqAmount * quantity >= haveAmount;
    }
}