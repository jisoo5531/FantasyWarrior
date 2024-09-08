using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
   
    /// <summary>
    /// ���� ���� ���Կ� �ִ� �������� ID
    /// </summary>
    private int itemID;
    /// <summary>
    /// ���� Ŭ�� Ȯ���� ���� ����
    /// </summary>
    private float clickTime;   

    /// <summary>
    /// <para>���� �ʱ�ȭ</para>
    /// ���� �������� ID�� 0�̶�� �������� ���� �� ����.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="sprite"></param>
    public void Initialize(int itemID = 0, Sprite sprite = null)
    {
        this.itemID = itemID;
        Debug.Log(sprite == null);        
        if (itemID == 0)
        {            
            this.itemImage.ImageTransparent(0);
        }
        else
        {            
            this.itemImage.sprite = sprite;
            this.itemImage.ImageTransparent(1);
        }
    }
    /// <summary>
    /// IPointerClickHandler �������̽�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {        
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
        {
            clickTime = Time.time;
        }
    }
    /// <summary>
    /// ����Ŭ������ ��, ��� ����. ������ return
    /// </summary>
    private void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }
        Debug.Log("����Ŭ��. ������ ����");
        Debug.Log($"���� ������ ID : {itemID}");
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemFromDB(itemID);
        string part = PlayerEquipManager.EquipParts[(int)equipItem.Equip_Type];

        PlayerEquipManager.Instance.UnEquip(part, this.itemID);

        this.itemImage.ImageTransparent(0);
        this.itemID = 0;
    }
}
