using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemQuantityText;

    // TODO : 딕셔너리 초기화, 아이템을 얻어서 수량에 변화가 생기면 인벤토리 UI도 변화가 생기게끔 event?로 구현 InventoryManager에 OnGetItem 이벤트 활용    
    public void Initialize(Sprite sprite, int quantity)
    {
        this.itemIcon.sprite = sprite;
        itemQuantityText.text = quantity.ToString();
        itemIcon.ImageTransparent(1);
        itemQuantityText.gameObject.SetActive(true);
    }
}
