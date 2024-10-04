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
    /// 아이템 제작을 할 때 호출할 메서드
    /// <para>수량만큼 제작</para>
    /// </summary>
    /// <param name="craftItem"></param>
    /// <param name="amount"></param>
    public void CraftItem(int userID, ItemData craftItem, int amount, Action success)
    {        
        // TODO  : 얘 아이디 고쳐
        InventoryManager.Instance.GetItem(userID, craftItem, amount);        
        success?.Invoke();
    }    
}
