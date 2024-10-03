using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    [Header("MonoBehaviour ����")]
    public Animator playerAnim;
    public GameObject actionPregressBar;    

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
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        Debug.Log("���Ⱑ ��?");
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
