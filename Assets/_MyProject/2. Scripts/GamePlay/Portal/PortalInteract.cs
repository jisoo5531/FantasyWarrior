using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalInteract : MonoBehaviour
{
    public LayerMask playerLayer;
    /// <summary>
    /// 상호작용 윈도우
    /// </summary>
    public GameObject InteractAction;
    public GameObject portalParticle;
    

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteractFishing;
    }
    //private void OnDisable()
    //{
    //    GameManager.inputActions.PlayerActions.Interact.performed -= OnInteractFishing;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        portalParticle.SetActive(true);
        InteractAction.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        InteractAction.SetActive(false);
        portalParticle.SetActive(false);
    }

    private void OnInteractFishing(InputAction.CallbackContext context)
    {
        if (true == InteractAction.activeSelf)
        {
            transform.parent.GetComponent<Portal>().ActivePortal();
            InteractAction.SetActive(false);            
        }
    }
}
