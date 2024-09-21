using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : MonoBehaviour
{
    public float jumpForce = 10f;  // ���� ���� ũ�� ����
    public float groundCheckDistance = 1.0f;  // �ٴ� ���� �Ÿ�
    public float maxJumpHeight = 3.0f;  // �ִ� ���� ����
    public float fallMultiplier = 2.5f;  // �ϰ� �� �߷� ���ӵ� ���
    public float additionalFallForce = 3f;  // �ϰ� �� �߰����� ��

    private Animator anim;
    private Rigidbody rb;
    public LayerMask groundLayer;  // Ground ���̾�

    public bool isGrounded;
    private bool isLanding;
    private Vector3 initialPosition;  // ���� ���� ��ġ

    [Header("Feedbacks")]
    /// a MMF_Player to play when the Hero starts jumping
    public MMF_Player JumpFeedback;
    /// a MMF_Player to play when the Hero lands after a jump
    public MMF_Player LandingFeedback;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �� �����Ӹ��� �ٴ� ����
        CheckGroundProximity();

        // ���� �Է��� �޾��� ��, ���� ���� ��쿡�� ����
        if (Input.GetKeyDown(KeyCode.Alpha1) && isGrounded)
        {
            Jump();
        }

        // ���� ���̰� �ִ� ���̸� ������ ������ �ӵ��� ����
        if (!isGrounded && transform.position.y > initialPosition.y + maxJumpHeight)
        {
            LimitJumpHeight();
        }
        // �ϰ� ���� �� �߷� ���ӵ��� ���̰� �߰����� ���� ��
        if (rb.velocity.y < 0)
        {
            // �ϰ� �ӵ� ����
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // �߰����� �ϰ� �� ����
            rb.AddForce(Vector3.down * additionalFallForce);
        }
    }

    private void Jump()
    {
        // ���� ���� �� ���� ��ġ ����
        initialPosition = transform.position;

        // ���� �ִϸ��̼� ����
        anim.SetTrigger("jump");                
    }

    public void OnUpforce()
    {
        // �������� ���ϰ� ���� �߰�
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // ���� �� ���� ���� ����
        isLanding = false;
    }

    // �ִ� ���� ���̸� ������ �ӵ��� ����
    private void LimitJumpHeight()
    {
        // ���� �ö󰡴� �ӵ��� 0���� �����Ͽ� �� �̻� �ö��� �ʵ��� ��
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    // ĳ���� �Ʒ��� ���� ������ �ִ��� Ȯ��
    private void CheckGroundProximity()
    {
        RaycastHit hit;
        // ĳ������ �Ʒ��� Raycast�� ��
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // ���� �Ÿ� ���� �ٴ��� ������ ���� �ִϸ��̼� ���� �غ�
            if (!isGrounded && !isLanding && rb.velocity.y < 0)  // �ϰ� ���� ����
            {
                isLanding = true;  // ���� �ִϸ��̼� ����
                anim.SetTrigger("Land");
            }
        }
        else
        {
            isLanding = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LandingFeedback?.PlayFeedbacks();
            isGrounded = true; // �ٴڿ� ������ �ٽ� ���� ����
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
