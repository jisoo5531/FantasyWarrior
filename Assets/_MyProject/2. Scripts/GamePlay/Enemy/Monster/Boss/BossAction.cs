using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum B_SkillAction
{
    RockShooting,
    Jump,
    Spin,
}

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

    /// <summary>
    /// ���� ��ų, ���� �׼��� �������� Ȯ���� ���� ����
    /// </summary>
    private bool isActionFinish = false;

    public event Action<int> OnSkillPlay;

    private void Awake()
    {
        boss = GetComponent<BossTest>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        StartCoroutine(BossActionCoroutine());
    }

    private IEnumerator BossActionCoroutine()
    {
        isActionFinish = false;
        Debug.Log($"{boss.followable.DistanceToPlayer}, {boss.attackable.Range}");
        if (boss.followable.DistanceToPlayer <= boss.attackable.Range)
        {
            int random = UnityEngine.Random.Range(0, 3);
            // �⺻ ���� ��Ÿ� ���� ������
            // �⺻ ���� �Ǵ� ���� ��ų �� �� �ϳ�
            switch (random)
            {
                case 0:
                case 1:
                    // �⺻����
                    BasicAttack();
                    break;
                case 2:
                    Rotate();
                    break;
                default:
                    break;
            }
        }
        else
        {
            int random = UnityEngine.Random.Range(0, 5);
            // �ٱ��� ������
            // ����, �� ���� ���� �� �ϳ�
            if (boss.followable.DistanceToPlayer > 14 && boss.followable.DistanceToPlayer < 17)
            {
                random = UnityEngine.Random.Range(0, 3);
            }
            switch (random)
            {
                case 0:
                case 1:
                    RockShootingAction();
                    break;
                case 2:
                    Jump();
                    break;
                case 3:
                case 4:
                    Invoke("IsActionFinish", 2f);
                    boss.IsNavStop(false, 0);
                    break;
                default:
                    break;
            }
        }

        yield return new WaitUntil(() => isActionFinish == true);

        StartCoroutine(BossActionCoroutine());
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

    #region �⺻����
    private void BasicAttack()
    {
        //transform.LookAt(boss.player.transform.position);        
        ActionStart();
        anim.SetTrigger("BasicAttack");
        anim.SetLayerWeight(1, 1);

        //boss.IsNavStop(false, 4f);
        Invoke("IsActionFinish", 5f);
    }
    #endregion

    #region ���� ����
    public void Jump()
    {
        // TODO : ���� ���� ������ ������ �ڷ� �з����� �ϱ�, ����Ż ����
        ActionStart();


        // ���� ���� �� ���� ��ġ ����
        initialPosition = transform.position;


        // ���� �ִϸ��̼� ����
        anim.SetTrigger("jump");
        ChargeFeedback?.PlayFeedbacks();
        OnSkillPlay?.Invoke((int)B_SkillAction.Jump);

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
                Invoke("ActionFinish", 2f);
                Invoke("IsActionFinish", 2f);
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
        ActionStart();

        RotateFeedback?.PlayFeedbacks();
        OnSkillPlay?.Invoke((int)B_SkillAction.Spin);

        GetComponent<BossSkill>().SkillPlay(0);
        //boss.IsNavStop(false, 1f);
        Invoke("SpinAttackFinish", 1f);
        Invoke("IsActionFinish", 2f);
    }
    private void SpinAttackFinish()
    {
        GetComponent<BossSkill>().SkillFinish(0);
    }

    #endregion

    #region �� ���� ����
    public void RockShootingAction()
    {
        ActionStart();
        transform.LookAt(boss.player.transform.position);
        anim.SetTrigger("ShardRock_Shooting");
        OnSkillPlay?.Invoke((int)B_SkillAction.RockShooting);
    }
    public void RockShooting()
    {
        RockShootingFeedback?.PlayFeedbacks();
        //boss.IsNavStop(false, 1.5f);
        Invoke("IsActionFinish", 3f);
    }
    #endregion    

    private void ActionStart()
    {
        boss.unitAnim.MoveAnimPlay(false);
        boss.isAction = true;
        boss.M_StateMachine.Stop();
        boss.nav.enabled = false;
    }
    private void ActionFinish()
    {
        boss.isAction = false;
        boss.M_StateMachine.Initialize(boss.M_StateMachine.idleState);
        boss.nav.enabled = true;
        anim.SetLayerWeight(1, 0);
    }
    private void IsActionFinish()
    {
        ActionFinish();
        isActionFinish = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isLanding)
            {
                LandingFeedback?.PlayFeedbacks();
            }
            isGrounded = true; // �ٴڿ� ������ �ٽ� ���� ����
        }
    }
}
