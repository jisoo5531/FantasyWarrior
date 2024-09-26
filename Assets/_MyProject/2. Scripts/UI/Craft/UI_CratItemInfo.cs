using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CratItemInfo : MonoBehaviour
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
    public Button craftButton;
    [Header("���� ��� ����Ʈ")]
    public GameObject craftMaterialContent;

    private int itemQuantity;

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
    public void Initialize(ItemData item)
    {
        itemQuantity = 0;
        quantityInputText.text = "0";
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{item.Item_Name}");
        playerGoldText.text = UserStatManager.Instance.userStatClient.Gold.ToString();
    }

    /// <summary>
    /// ���� Up ��ư Ŭ��
    /// </summary>
    private void OnClickAmountUpButton()
    {
        itemQuantity += 1;
        // ���� ��ᰡ �׸�ŭ ������ 
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
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    private void UpdateItemGoldText()
    {
        
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
        // ���� ������ŭ ��ᰡ ������ 
    }
    private void OnClickBackButton()
    {
        gameObject.SetActive(false);
    }
}
