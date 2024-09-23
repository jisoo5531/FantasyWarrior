using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{    
    public float actionTime = 2f;
    public Animator playerAnim;

    public void Action()
    {
        StartCoroutine(Fish());
    }
    private IEnumerator Fish()
    {
        playerAnim.SetBool("Fishing", true);
        yield return new WaitForSeconds(actionTime);
        playerAnim.SetBool("Fishing", false);

        GetItem();
    }
    private void GetItem()
    {
        // TODO : 임시 아이템 저장
        ItemData smallFish = ItemManager.Instance.GetItemData(9);
        InventoryManager.Instance.GetItem(smallFish, 1);
    }
}
