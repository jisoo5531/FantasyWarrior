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
        PlayerController.inputActions.PlayerActions.Inventory.performed += OnInventory;
        PlayerController.inputActions.PlayerActions.Inventory.canceled += OnInventory;
    }
    private void OnDisable()
    {
        PlayerController.inputActions.PlayerActions.Inventory.performed -= OnInventory;
        PlayerController.inputActions.PlayerActions.Inventory.canceled -= OnInventory;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        isOpenInventory = context.ReadValueAsButton();
    }
}
