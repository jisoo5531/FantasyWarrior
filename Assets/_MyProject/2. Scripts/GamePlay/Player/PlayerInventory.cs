using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private bool isOpenInventory = false;

    // TODO : �κ��丮 ���� �ݱ� - UI �� ����
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
