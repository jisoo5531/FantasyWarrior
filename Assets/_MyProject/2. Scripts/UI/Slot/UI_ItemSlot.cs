using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    /// <summary>
    /// ���� ������ �̹���
    /// </summary>
    public Image itemImage;
    public UI_ItemInfo itemInfoWindow;
    /// <summary>
    /// ���� �κ��丮 ���Կ� �ִ� �������� ID
    /// </summary>
    public int itemID = 0;
    /// <summary>
    /// ���� Ŭ�� Ȯ���� ���� ����
    /// </summary>
    protected float clickTime = 0;

    protected int userId;

    public virtual void Initialize(int userId, int itemID, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        this.userId = userId;
        this.itemID = itemID;
        this.itemImage.sprite = sprite;
        this.itemInfoWindow = itemInfo;        
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
    /// <para>����Ŭ������ �� ��� �������̸� ����, ����</para>
    /// <para>��Ÿ �������̳� ���Կ� �ƹ��͵� ������ return</para>
    /// TODO : �Һ� �������� ��쵵 �Ŀ� ����.
    /// </summary>
    protected virtual void OnMouseDoubleClick()
    {
        // override
    }
    /// <summary>
    /// <para>IPointerEnterHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ��� �������� ������ �����Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoWindow == null || itemID == 0)
        {
            return;
        }

        itemInfoWindow.gameObject.SetActive(true);
        itemInfoWindow.Initialize(this.userId, this.itemID);
    }
    /// <summary>
    /// <para>IPointerExitHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ���� �������� ������ ������Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfoWindow == null || itemID == 0)
        {
            return;
        }
        itemInfoWindow.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.itemID == 0)
        {
            return;
        }
        itemInfoWindow.gameObject.GetComponent<RectTransform>().position = eventData.position + new Vector2(250, -50);
    } 
}
