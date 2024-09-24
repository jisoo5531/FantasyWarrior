using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GetItemPrefab : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemName;

    public void Initialize(ItemData item)
    {
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{item.Item_Name}");
        itemName.text = item.Item_Name;

        Destroy(gameObject, 3f);
    }
}
