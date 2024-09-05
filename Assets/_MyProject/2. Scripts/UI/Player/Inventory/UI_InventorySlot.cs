using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    private Image itemIcon;
    private TMP_Text itemQuantityText;

    // TODO : 딕셔너리 초기화, 아이템을 얻어서 수량에 변화가 생기면 인벤토리 UI도 변화가 생기게끔 event?로 구현 InventoryManager에 OnGetItem 이벤트 활용    
    public void Initialize(Dictionary<Image, Sprite> imgParam, Dictionary<TMP_Text, string> txtParam)
    {
        //this.itemIcon = imgParam.Keys;
        //itemIcon.sprite = imgParam.Values;
        //this.itemQuantityText = text;                

    }


}
