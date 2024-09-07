using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipSlot : MonoBehaviour
{
    public Image itemIcon;

    public void Initialize(Sprite itemImage)
    {
        this.itemIcon.sprite = itemImage;
    }
}
