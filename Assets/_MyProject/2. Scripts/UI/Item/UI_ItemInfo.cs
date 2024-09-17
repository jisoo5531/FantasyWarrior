using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemInfo : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemNameText;
    protected List<ItemData> itemDataList;
    protected ItemData itemData;
    protected string itemName;

    public virtual void Initialize(int itemID)
    {
        itemDataList = ItemManager.Instance.itemDataList;        
        itemData = itemDataList.Find((x) => { return x.Item_ID.Equals(itemID); });
        itemName = itemData.Item_Name;
        itemNameText.text = itemName;
        itemImage.sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
    }
}
