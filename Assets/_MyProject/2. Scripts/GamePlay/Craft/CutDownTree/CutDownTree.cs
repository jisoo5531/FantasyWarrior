using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutDownTree : Craft
{
    private float actionTime = 2f;    
    protected override IEnumerator ActionCoroutine()
    {
        playerAnim.SetBool("CutDownTree", true);
        OnProgressBar(this.actionTime);
        yield return new WaitForSeconds(actionTime);
        playerAnim.SetBool("CutDownTree", false);        
        yield return base.ActionCoroutine();
    }

    protected override void GetItem()
    {
        craftItem = ItemManager.Instance.GetItemData(1);
        craftItemAmount = 1;
        base.GetItem();
    }
}
