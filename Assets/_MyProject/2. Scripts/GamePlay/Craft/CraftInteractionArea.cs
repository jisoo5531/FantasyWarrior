using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftInteractionArea : MonoBehaviour
{
    public LayerMask playerLayer;
    /// <summary>
    /// 상호작용 윈도우
    /// </summary>
    public GameObject InteractAction;

    public CraftType craftType;

    public Craft craft;


    public bool isHaveTool;

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteractFishing;
    }
    private void Start()
    {
        // TODO : 나중에 아이템을 팔거나 버리는 기능 넣을 시, 그때에도 체크해주도록
        InventoryManager.Instance.OnGetCraftItem += CheckUserHaveTool;
        CheckUserHaveTool();
    }
    protected virtual void CheckUserHaveTool()
    {
        UserCraftToolData userCraftTool = InventoryManager.Instance.userCraftToolClient.Find(x => x.CreftType.Equals(craftType));
        isHaveTool = userCraftTool.Item_ID != 0;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Interact.performed -= OnInteractFishing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        InteractAction.SetActive(isHaveTool);
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
            InteractAction.SetActive(false);
            craft.Action();
        }
    }
}
