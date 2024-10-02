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
    private float rotationSpeed = 100f;
    private Vector3 clickPos;
    private Vector2 moveInput;
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    [HideInInspector] public CharacterController controller;

    private bool isRun = false;

    // �÷��̾ NavMeshAgent�� �Ҵ�
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        Debug.Log("여기 몇 번? ");
        
    }
    void Start()
    {
        Debug.Log("여기 되나?");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        // 마우스 오른쪽 버튼을 클릭한 경우
        if (Input.GetMouseButtonDown(1))
        {
            // 에이전트가 움직일 수 있도록 설정
            agent.isStopped = false;
            Debug.Log("오른쪽 클릭");

            // 카메라에서 마우스 클릭 지점으로 레이캐스트 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 클릭 지점이 NavMesh 위에 있는지 확인
                if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out UnityEngine.AI.NavMeshHit navHit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    clickPos = navHit.position; // 클릭된 NavMesh 좌표 저장
                    agent.SetDestination(navHit.position); // 에이전트가 목적지로 이동하도록 설정
                }
            }
        }

        // 에이전트가 목적지에 도착했는지 확인
        CheckArriveDestination();

        // 에이전트의 속도를 기반으로 애니메이션 제어
        float speed = agent.velocity.magnitude;
        GetComponent<PlayerAnimation>().MoveAnimation(speed);
    }

    private void CheckArriveDestination()
    {
        // 경로 계산이 완료되었고, 남은 거리가 stoppingDistance 이하인 경우
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 경로가 없거나 에이전트가 멈춘 경우 도착한 것으로 간주
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                Debug.Log("Agent has arrived at the destination");
                agent.ResetPath(); // 경로 리셋
                agent.isStopped = true; // 에이전트 이동 중지
            }
        }
    }


    private void OnEnable()
    {
        EventHandler.sceneEvent.RegisterSceneOut(OffNavAgent);
        EventHandler.sceneEvent.RegisterSceneIn(OnNavAgent);
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
        EventHandler.sceneEvent.UnRegisterSceneOut(OffNavAgent);
        EventHandler.sceneEvent.UnRegisterSceneIn(OnNavAgent);
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
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }
    }
    private void OffNavAgent()
    {
        agent.enabled = false;
    }
    private void OnNavAgent()
    {
        agent.enabled = true;
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
        isRun = context.ReadValueAsButton();
    }
}
