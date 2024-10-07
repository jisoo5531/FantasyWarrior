using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
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
    public MMF_Player ChargeFeedback;
    public MMF_Player RotateFeedback;
    public MMF_Player RockShootingFeedback;

    private BossTest boss;
    private BossStateMachine bossStateMachine;

    private void Awake()
    {
        boss = GetComponent<BossTest>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (boss.M_StateMachine is BossStateMachine bossStateMachine)
        {
            this.bossStateMachine = bossStateMachine;    
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Rotate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RockShootingAction();
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
    #region ���� ����
    public void Jump()
    {
        boss.IsNavStop(true, 0);
        // ���� ���� �� ���� ��ġ ����
        initialPosition = transform.position;

        // ���� �ִϸ��̼� ����
        anim.SetTrigger("jump");
        ChargeFeedback?.PlayFeedbacks();
        boss.IsNavStop(false, 4f);
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

    #endregion

    #region ȸ�� ����

    public void Rotate()
    {
        boss.nav.isStopped = true;
        RotateFeedback?.PlayFeedbacks();
        boss.IsNavStop(false, 1f);
    }

    #endregion

    #region �� ���� ����
    public void RockShootingAction()
    {
        boss.nav.isStopped = true;
        anim.SetTrigger("ShardRock_Shooting");

    }
    public void RockShooting()
    {
        RockShootingFeedback?.PlayFeedbacks();
        boss.IsNavStop(false, 1.5f);
    }
    #endregion    

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
