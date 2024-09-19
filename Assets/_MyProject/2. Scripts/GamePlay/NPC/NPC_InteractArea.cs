using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC_InteractArea : MonoBehaviour
{
    public LayerMask playerLayer;
    public new Collider collider;
    public GameObject InteractAction;
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
            Debug.Log($"상호작용 했나?. {isInteracted}");
        }        
    }
}
