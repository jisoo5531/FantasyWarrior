using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UI_PlayerGetItem : MonoBehaviour
{
    public GameObject UI_GetitemPrefab;
    public GameObject GetItemContent;

    private int userId;
    private void Start()
    {
        // 서버에서 실행되지 않도록
        if (NetworkServer.active)
        {
            return; // 서버에서는 UI를 실행하지 않음
        }
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        this.userId = DatabaseManager.Instance.GetPlayerData(transform.root.gameObject).UserId;
        GameManager.Instance.invenManger[this.userId].OnGetItemData += OnGetItem;        
    }

    private void OnGetItem(ItemData item, int userID)
    {        
        if (userId == userID)
        {
            GameObject go_getItem = Instantiate(UI_GetitemPrefab, GetItemContent.transform);
            go_getItem.GetComponent<UI_GetItemPrefab>().Initialize(item);
        }        
    }
}
