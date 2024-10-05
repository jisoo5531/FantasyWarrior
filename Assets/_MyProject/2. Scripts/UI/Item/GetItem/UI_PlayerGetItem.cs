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
        // �������� ������� �ʵ���
        if (NetworkServer.active)
        {
            return; // ���������� UI�� �������� ����
        }
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
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
