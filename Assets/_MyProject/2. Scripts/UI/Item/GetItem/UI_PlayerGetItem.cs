using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerGetItem : MonoBehaviour
{
    public GameObject UI_GetitemPrefab;
    public GameObject GetItemContent;

    private void Start()
    {
        InventoryManager.Instance.OnGetItemData += OnGetItem;
    }

    private void OnGetItem(ItemData item)
    {
        GameObject go_getItem = Instantiate(UI_GetitemPrefab, GetItemContent.transform);
        go_getItem.GetComponent<UI_GetItemPrefab>().Initialize(item);
    }
}
