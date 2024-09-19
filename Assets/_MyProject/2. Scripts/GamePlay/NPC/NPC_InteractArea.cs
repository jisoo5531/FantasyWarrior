using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_InteractArea : MonoBehaviour
{
    /// <summary>
    /// ���� NPC�� ID
    /// </summary>
    public int NPC_ID = 1;
    /// <summary>
    /// ��ȣ�ۿ� ������
    /// </summary>
    public GameObject InteractAction;
    /// <summary>
    /// ��ȣ�ۿ��ϸ� ������ ��ȭâ
    /// </summary>
    public GameObject Dialog;
    public LayerMask playerLayer;    
    private bool isInteracted;

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteractNPC;        
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Interact.performed -= OnInteractNPC;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        InteractAction.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        InteractAction.SetActive(false);
    }

    private void OnInteractNPC(InputAction.CallbackContext context)
    {
        if (true == InteractAction.activeSelf)
        {
            isInteracted = context.ReadValueAsButton();
            Dialog.SetActive(true);
        }        
    }
}
