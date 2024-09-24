using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : Craft
{
    private float actionTime = 2f;
    protected override IEnumerator ActionCoroutine()
    {

        playerAnim.SetBool("Fishing", true);
        yield return new WaitForSeconds(actionTime);
        playerAnim.SetBool("Fishing", false);
        actionTime = 2f;
        yield return base.ActionCoroutine();
    }

    protected override void GetItem()
    {
        craftItem = ItemManager.Instance.GetItemData(9);
        craftItemAmount = 1;
        base.GetItem();
    }
}
