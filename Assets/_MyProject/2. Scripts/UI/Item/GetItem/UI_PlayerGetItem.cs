using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerGetItem : MonoBehaviour
{
    public GameObject UI_GetitemPrefab;
    public GameObject GetItemContent;


    //private void Start()
    //{
    //    InventoryManager.Instance.OnGetItemData += OnGetItem;
    //}
    private void Awake()
    {
        InventoryManager.Instance.OnGetItemData += OnGetItem;
    }
    private void OnDestroy()
    {
        InventoryManager.Instance.OnGetItemData -= OnGetItem;
    }

    private void OnGetItem(ItemData item)
    {
        Debug.Log("아이템 이름 : " + item.Item_Name);
        Debug.Log(UI_GetitemPrefab == null);
        //Debug.Log()
        Debug.Log(this == null);
        Debug.Log(GetItemContent == null);
        GameObject go_getItem = Instantiate(UI_GetitemPrefab, transform);
        go_getItem.GetComponent<UI_GetItemPrefab>().Initialize(item);
    }
}
