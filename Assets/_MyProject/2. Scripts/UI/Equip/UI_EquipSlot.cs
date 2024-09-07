using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipSlot : MonoBehaviour
{
    public Image itemImage;

    public void Initialize(Sprite sprite = null)
    {
        Debug.Log(sprite == null);
        if (sprite == null)
        {            
            this.itemImage.ImageTransparent(0);
        }
        else
        {            
            this.itemImage.sprite = sprite;
            this.itemImage.ImageTransparent(1);
        }
    }
}
