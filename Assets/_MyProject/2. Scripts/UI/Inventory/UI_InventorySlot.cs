using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemQuantityText;
    
    public void Initialize(Sprite sprite, int quantity)
    {
        this.itemIcon.sprite = sprite;
        itemQuantityText.text = quantity.ToString();
        itemIcon.ImageTransparent(1);
        itemQuantityText.gameObject.SetActive(true);
    }
}
