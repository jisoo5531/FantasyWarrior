using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopSellItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemQuantityText;

    public void Initialize(InventoryData item, Sprite itemSprite)
    {
        itemIcon.ImageTransparent(1);
        itemIcon.sprite = itemSprite;
        itemQuantityText.gameObject.SetActive(true);
        itemQuantityText.text = item.Quantity.ToString();
    }
    public void SlotClear()
    {
        itemIcon.ImageTransparent(0);
        itemQuantityText.gameObject.SetActive(false);
    }
}
