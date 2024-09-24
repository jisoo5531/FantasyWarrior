using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public Animator playerAnim;
    /// <summary>
    /// 해당 액션으로 얻는 아이템
    /// </summary>
    protected ItemData craftItem;
    /// <summary>
    /// 얻을 아이템의 개수
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
