using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_CraftMaterialInfo : MonoBehaviour
{
    [Header("��� ����")]
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text ReqAmountText;
    public TMP_Text playerHaveAmountText;
    

    private ItemData materialItem;
    private RecipeMaterialData recipeMaterial;

    private Action<int, int, int> postInit;

    private int userID;

    public void Initialize(int userId, RecipeMaterialData recipeMaterial, Action<int, int, int> init)
    {
        this.userID = userId;
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

        int? haveAmount = GameManager.Instance.invenManager[userID].GetItemQuantity(materialItem.Item_ID);        
        if (haveAmount != null)
        {
            playerHaveAmountText.text = $"x{haveAmount.ToString()}";
            postInit?.Invoke(materialItem.Item_ID, recipeMaterial.M_Quantity, haveAmount.Value);
        }        
    }
}
/// <summary>
/// ���� ������ üũ�ϱ� ���� Ŭ����
/// </summary>
public class C_MaterialPossesion
{
    public int itemID { get; set; }
    public int reqAmount { get; set; }
    public int haveAmount { get; set; }

    public C_MaterialPossesion(int itemID, int reqAmount, int haveAmount)
    {        
        this.itemID = itemID;
        this.reqAmount = reqAmount;
        this.haveAmount = haveAmount;
    }
    /// <summary>
    /// ���� �� �ִ� �ִ� ����
    /// </summary>
    /// <returns></returns>
    public int MaxAmount()
    {        
        return haveAmount / reqAmount;
    }
    /// <summary>
    /// ������ŭ ���� �� �ִ��� üũ
    /// <para>�Ű������� ���� ����</para>
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public bool CheckAmount(int quantity)
    {
        return reqAmount * quantity >= haveAmount;
    }
}