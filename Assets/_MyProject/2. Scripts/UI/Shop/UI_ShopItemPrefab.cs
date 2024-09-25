using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopItemPrefab : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemNameText;
    [Header("아이템 상세정보 열람 버튼")]
    public Button ItemInfoButton;

    private void Awake()
    {
        Debug.Log("되지?");
        ItemInfoButton.onClick.AddListener(OnClickItemInfoButton);
    }
    public void Initialize(ItemData item)
    {
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{item.Item_Name}");
        itemNameText.text = item.Item_Name;
    }
    /// <summary>
    /// 이 아이템(오브젝트)를 클릭했을 때 상세 정보가 나오게 하기
    /// </summary>
    private void OnClickItemInfoButton()
    {
        Debug.Log("상점 아이템 상세정보 나오자.");
    }    
}
