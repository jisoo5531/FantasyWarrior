using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishInteract : MonoBehaviour
{
    public LayerMask playerLayer;
    /// <summary>
    /// 상호작용 윈도우
    /// </summary>
    public GameObject InteractAction;

    public Fishing fishing;
    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteractFishing;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Interact.performed -= OnInteractFishing;
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
    private void OnInteractFishing(InputAction.CallbackContext context)
    {
        if (true == InteractAction.activeSelf)
        {
            Debug.Log("낚시하자.");
            fishing.Action();
        }
    }
}
