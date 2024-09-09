using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // TODO : �÷��̾� ������ ���콺�� �غ���
    private float initSpeed = 0f;
    private float walkSpeed = 6f;
    private float runSpeed = 10f;
    private float gravity = -9.81f;
    private float jumpHeight = 1.5f;
    private float rotationSpeed = 100f; // ȸ�� �ӵ�
    private Vector2 moveInput;
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    [HideInInspector] public CharacterController controller;

    private bool isRun = false;

    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.Move.performed += OnMovePerformed;
        GameManager.inputActions.PlayerActions.Move.canceled += OnMovePerformed;

        GameManager.inputActions.PlayerActions.Run.performed += OnRunPerformed;
        GameManager.inputActions.PlayerActions.Run.canceled += OnRunPerformed;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Move.performed -= OnMovePerformed;
        GameManager.inputActions.PlayerActions.Move.canceled -= OnMovePerformed;

        GameManager.inputActions.PlayerActions.Run.performed -= OnRunPerformed;
        GameManager.inputActions.PlayerActions.Run.canceled -= OnRunPerformed;
    }

    public void Move(CharacterController controller)
    {
        this.controller = controller;        
        CheckGrounded();

        float moveSpeed = initSpeed;

        moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        
        if (moveDir.x != 0 || moveDir.z != 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Time.deltaTime * 30f);
            controller.Move(isRun ? moveDir * runSpeed * Time.deltaTime : moveDir * moveSpeed * Time.deltaTime);
            Rotate(moveDir);
        }

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }

    private void CheckGrounded()
    {
        // ���� ��� �ִ��� Ȯ��
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ���� ���� ���� �ӵ� �ʱ�ȭ
        }
    }
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnMoveCanceld(InputAction.CallbackContext context)
    {
        moveDir = Vector2.zero;
    }
    public void OnRunPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("�޸���.");
        isRun = context.ReadValueAsButton();
    }
}
