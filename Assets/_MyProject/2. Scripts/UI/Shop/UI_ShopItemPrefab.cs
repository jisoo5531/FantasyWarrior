using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopItemPrefab : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemNameText;
    [Header("������ ������ ���� ��ư")]
    public Button ItemInfoButton;

    private void Awake()
    {
        Debug.Log("����?");
        ItemInfoButton.onClick.AddListener(OnClickItemInfoButton);
    }
    public void Initialize(ItemData item)
    {
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{item.Item_Name}");
        itemNameText.text = item.Item_Name;
    }
    /// <summary>
    /// �� ������(������Ʈ)�� Ŭ������ �� �� ������ ������ �ϱ�
    /// </summary>
    private void OnClickItemInfoButton()
    {
        Debug.Log("���� ������ ������ ������.");
    }    
}
