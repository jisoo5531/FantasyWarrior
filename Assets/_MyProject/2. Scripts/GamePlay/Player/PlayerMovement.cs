using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float initSpeed = 0f;
    private float walkSpeed = 6f;
    private float runSpeed = 10f;
    private float gravity = -9.81f;
    private float jumpHeight = 1.5f;
    private float rotationSpeed = 100f; // ȸ�� �ӵ�
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    private CharacterController controller;


    public void Move(Transform unitTrans, CharacterController controller, float horizontal, float vertical, bool isRun)
    {
        this.controller = controller;
        CheckGrounded();

        moveDir = new Vector3(horizontal, 0, vertical);

        float moveSpeed = initSpeed;

        if (horizontal != 0 || vertical != 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, Time.deltaTime * 30f);
            controller.Move(isRun ? moveDir * runSpeed * Time.deltaTime : moveDir * moveSpeed * Time.deltaTime);
            Rotate(unitTrans, moveDir);
        }

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Rotate(Transform unitTrans, Vector3 direction)
    {
        unitTrans.rotation = Quaternion.Lerp(unitTrans.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
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
}
