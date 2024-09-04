using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private bool isOpenInventory = false;

    // TODO : 인벤토리 열고 닫기 - UI 와 연동
    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.UI_Inventory.performed += OnInventory;
        GameManager.inputActions.PlayerActions.UI_Inventory.canceled += OnInventory;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.UI_Inventory.performed -= OnInventory;
        GameManager.inputActions.PlayerActions.UI_Inventory.canceled -= OnInventory;
    }

    private void OnInventory(InputAction.CallbackContext context)
    {
        isOpenInventory = context.ReadValueAsButton();
    }
}
