using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public Animator playerAnim;
    /// <summary>
    /// �ش� �׼����� ��� ������
    /// </summary>
    protected ItemData craftItem;
    /// <summary>
    /// ���� �������� ����
    /// </summary>
    protected int craftItemAmount;    
    public void Action()
    {
        StartCoroutine(ActionCoroutine());
    }
    protected virtual IEnumerator ActionCoroutine()
    {
        yield return null;

        GetItem();
    }
    protected virtual void GetItem()
    {        
        InventoryManager.Instance.GetItem(craftItem, 1);
    }
}
