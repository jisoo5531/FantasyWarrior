using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CratItemInfo : MonoBehaviour
{
    [Header("아이템 이미지")]
    public Image itemIcon;
    [Header("골드")]
    public TMP_Text playerGoldText;
    public TMP_Text itemGoldText;
    [Header("수량 Input")]
    public TMP_InputField quantityInputText;
    [Header("버튼")]
    public Button backButton;
    public Button Q_UpButton;
    public Button Q_DownButton;
    public Button craftButton;
    [Header("제작 재료 리스트")]
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
    /// 수량 Up 버튼 클릭
    /// </summary>
    private void OnClickAmountUpButton()
    {
        itemQuantity += 1;
        // 만약 재료가 그만큼 없으면 
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
