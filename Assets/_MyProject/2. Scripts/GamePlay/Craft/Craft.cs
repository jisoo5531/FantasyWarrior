using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [Header("MonoBehaviour 참조")]
    public Animator playerAnim;
    public GameObject actionPregressBar;    

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
    protected void OnProgressBar(float actionTime)
    {        
        actionPregressBar.SetActive(true);
        actionPregressBar.GetComponent<UI_CraftTime>().Initalize(actionTime);
    }
}
