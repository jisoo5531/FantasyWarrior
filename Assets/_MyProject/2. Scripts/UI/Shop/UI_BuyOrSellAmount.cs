using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BuyOrSellAmount : MonoBehaviour
{
    [Header("������ �̹���")]
    public Image itemIcon;
    [Header("���")]
    public TMP_Text playerGoldText;
    public TMP_Text itemGoldText;
    [Header("���� Input")]
    public TMP_InputField quantityInputText;
    [Header("��ư")]
    public Button backButton;    
    public Button Q_UpButton;
    public Button Q_DownButton;

    private ItemData itemData;    

    /// <summary>
    /// ��ų� �Ȱ��� �ϴ� �������� ����
    /// </summary>
    protected int itemQuantity = 0;

    private void Awake()
    {
        quantityInputText.onValueChanged.AddListener(OnValueChangedInputQuantity);
        ButtonInitialize();
    }
    protected virtual void ButtonInitialize()
    {
        Q_UpButton.onClick.AddListener(OnClickAmountUpButton);
        Q_DownButton.onClick.AddListener(OnClickAmountDownButton);
        backButton.onClick.AddListener(OnClickBackButton);
    }
    protected void Initialize(int userId, ItemData item)
    {
        itemQuantity = 0;
        quantityInputText.text = "0";
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{item.Item_Name}");
        playerGoldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }
       
    /// <summary>
    /// ���� Up ��ư Ŭ��
    /// </summary>
    protected virtual void OnClickAmountUpButton()
    {
        itemQuantity += 1;
    }
    /// <summary>
    /// ���� Down ��ư Ŭ��
    /// </summary>
    protected virtual void OnClickAmountDownButton()
    {
        itemQuantity -= 1;
        if (itemQuantity < 0)
        {
            itemQuantity = 0;
        }        
    }
    protected virtual void UpdateItemGoldText()
    {
        // override
    }
    protected void UpdateQuantityText()
    {        
        quantityInputText.text = itemQuantity.ToString();
    }
    /// <summary>
    /// ������ ������ input���� �Է��Ͽ� ���� ����� �� ȣ��
    /// </summary>
    /// <param name="value"></param>
    protected virtual void OnValueChangedInputQuantity(string value)
    {
        itemQuantity = int.Parse(value);
    }    
    private void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
