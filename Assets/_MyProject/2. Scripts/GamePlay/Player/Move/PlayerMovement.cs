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
    private Vector2 moveInput;
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    [HideInInspector] public CharacterController controller;

    private bool isRun = false;

    // �÷��̾ NavMeshAgent�� �Ҵ�
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        Debug.Log("여기 되나?");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {        
        if (Input.GetMouseButtonDown(1))
        {
            agent.isStopped = false;
            Debug.Log("왼쪽 클릭");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {                
                if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out UnityEngine.AI.NavMeshHit navHit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    Debug.Log(navHit.position);
                    Debug.Log(agent == null);
                    agent.SetDestination(navHit.position);
                }
            }
            
        }
        float speed = agent.velocity.magnitude; // NavMeshAgent의 속도
        GetComponent<PlayerAnimation>().MoveAnimation(speed);        
    }
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
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
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
