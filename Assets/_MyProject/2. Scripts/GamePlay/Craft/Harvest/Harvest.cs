using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : Craft
{
    private float actionTime = 2f;
    protected override IEnumerator ActionCoroutine()
    {
        playerAnim.SetBool("Harvest", true);
        OnProgressBar(this.actionTime);
        yield return new WaitForSeconds(actionTime);
        playerAnim.SetBool("Harvest", false);
        yield return base.ActionCoroutine();
    }

    protected override void GetItem()
    {
        craftItem = ItemManager.Instance.GetItemData(14);
        craftItemAmount = 1;
        base.GetItem();
    }
}
