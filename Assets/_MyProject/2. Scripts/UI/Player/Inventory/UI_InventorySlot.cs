using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    private Image itemIcon;
    private TMP_Text itemQuantityText;

    // TODO : ��ųʸ� �ʱ�ȭ, �������� �� ������ ��ȭ�� ����� �κ��丮 UI�� ��ȭ�� ����Բ� event?�� ���� InventoryManager�� OnGetItem �̺�Ʈ Ȱ��    
    public void Initialize(Dictionary<Image, Sprite> imgParam, Dictionary<TMP_Text, string> txtParam)
    {
        //this.itemIcon = imgParam.Keys;
        //itemIcon.sprite = imgParam.Values;
        //this.itemQuantityText = text;                

    }


}
