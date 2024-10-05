using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BuyOrSellAmount : MonoBehaviour
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

    private ItemData itemData;    

    /// <summary>
    /// 사거나 팔고자 하는 아이템의 수량
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
    /// 수량 Up 버튼 클릭
    /// </summary>
    protected virtual void OnClickAmountUpButton()
    {
        itemQuantity += 1;
    }
    /// <summary>
    /// 수량 Down 버튼 클릭
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
    /// 아이템 수량을 input으로 입력하여 값이 변경될 때 호출
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
