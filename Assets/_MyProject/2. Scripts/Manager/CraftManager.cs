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
    public void CraftItem(ItemData craftItem, int amount, Action success)
    {        
        InventoryManager.Instance.GetItem(craftItem, amount);        
        success?.Invoke();
    }    
}
