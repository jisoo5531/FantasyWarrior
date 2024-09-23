using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemInfo : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemNameText;
    protected Dictionary<int, ItemData> item_Dict;
    protected ItemData itemData;
    protected string itemName;

    public virtual void Initialize(int itemID)
    {                
        itemData = ItemManager.Instance.GetItemData(itemID);
        itemName = itemData.Item_Name;
        itemNameText.text = itemName;
        itemImage.sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
    }
}
