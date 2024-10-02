using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTest : MonoBehaviour
{
    public float jumpForce = 10f;  // 점프 힘을 크게 설정
    public float groundCheckDistance = 1.0f;  // 바닥 감지 거리
    public float maxJumpHeight = 3.0f;  // 최대 점프 높이
    public float fallMultiplier = 2.5f;  // 하강 시 중력 가속도 배수
    public float additionalFallForce = 3f;  // 하강 시 추가적인 힘

    private Animator anim;
    private Rigidbody rb;
    public LayerMask groundLayer;  // Ground 레이어

    public bool isGrounded;
    private bool isLanding;
    private Vector3 initialPosition;  // 점프 시작 위치

    [Header("Feedbacks")]
    /// a MMF_Player to play when the Hero starts jumping
    public MMF_Player JumpFeedback;
    /// a MMF_Player to play when the Hero lands after a jump
    public MMF_Player LandingFeedback;
    public MMF_Player ChargeFeedback;
    public MMF_Player RotateFeedback;
    public MMF_Player RockShootingFeedback;

    public GameObject player;

    private NavMeshAgent nav;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Move();
    }

    private void Update()
    {        
        // 매 프레임마다 바닥 감지
        CheckGroundProximity();

        // 점프 입력을 받았을 때, 땅에 있을 경우에만 점프
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

        // 점프 중이고 최대 높이를 넘으면 위로의 속도를 줄임
        if (!isGrounded && transform.position.y > initialPosition.y + maxJumpHeight)
        {
            LimitJumpHeight();
        }
        // 하강 중일 때 중력 가속도를 높이고 추가적인 힘을 줌
        if (rb.velocity.y < 0)
        {
            // 하강 속도 가속
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // 추가적인 하강 힘 적용
            rb.AddForce(Vector3.down * additionalFallForce);
        }
    }
    public void Move()
    {
        nav.enabled = true;
        nav.isStopped = false;
        nav.SetDestination(player.transform.position);
    }

    #region 점프 공격
    private void Jump()
    {
        nav.isStopped = true;
        nav.enabled = false;
        // 점프 시작 시 현재 위치 저장
        initialPosition = transform.position;

        // 점프 애니메이션 실행
        anim.SetTrigger("jump");
        ChargeFeedback?.PlayFeedbacks();
    }
    public void Land()
    {
        transform.position = player.transform.position;
    }
    public void OnUpforce()
    {
        // 위쪽으로 강하게 힘을 추가
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // 점프 후 땅에 있지 않음
        isLanding = false;
    }
    // 최대 점프 높이를 넘으면 속도를 제어
    private void LimitJumpHeight()
    {
        // 위로 올라가는 속도를 0으로 설정하여 더 이상 올라가지 않도록 함
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    // 캐릭터 아래에 땅이 가까이 있는지 확인
    private void CheckGroundProximity()
    {
        RaycastHit hit;
        // 캐릭터의 아래로 Raycast를 쏨
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // 일정 거리 내에 바닥이 있으면 착지 애니메이션 실행 준비
            if (!isGrounded && !isLanding && rb.velocity.y < 0)  // 하강 중일 때만
            {
                isLanding = true;  // 착지 애니메이션 실행
                anim.SetTrigger("Land");
            }
        }
        else
        {
            isLanding = false;
        }
    }

    #endregion

    #region 회전 공격

    private void Rotate()
    {
        nav.isStopped = true;
        RotateFeedback?.PlayFeedbacks();
    }

    #endregion

    #region 돌 슈팅 공격
    private void RockShootingAction()
    {
        nav.isStopped = true;
        anim.SetTrigger("ShardRock_Shooting");
        
    }
    public void RockShooting()
    {        
        RockShootingFeedback?.PlayFeedbacks();
    }
    #endregion
    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            LandingFeedback?.PlayFeedbacks();
            isGrounded = true; // 바닥에 닿으면 다시 점프 가능
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
