using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void Initialize()
    {

    }

    /// <summary>
    /// ������ ������ �� �� ȣ���� �޼���
    /// <para>������ŭ ����</para>
    /// </summary>
    /// <param name="craftItem"></param>
    /// <param name="amount"></param>
    public void CraftItem(int userID, ItemData craftItem, int amount, Action success)
    {        
        // TODO  : �� ���̵� ����
        InventoryManager.Instance.GetItem(userID, craftItem, amount);        
        success?.Invoke();
    }    
}
