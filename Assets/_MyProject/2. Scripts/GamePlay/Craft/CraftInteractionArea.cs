using Mirror;
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

    private GameObject currentPlayer;    // 현재 상호작용 중인 플레이어
    public bool isHaveTool;
    

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteractFishing;
        EventHandler.playerEvent.RegisterPlayerEnter(CheckUserHaveTool);
    }
    private void Start()
    {
        int userId = DatabaseManager.Instance.GetPlayerData(currentPlayer).UserId;
        GameManager.Instance.invenManger[userId].OnGetCraftItem += CheckUserHaveTool;
        // TODO : 나중에 아이템을 팔거나 버리는 기능 넣을 시, 그때에도 체크해주도록        
        
    }
    protected virtual void CheckUserHaveTool(int userid)
    {
        UserCraftToolData userCraftTool = GameManager.Instance.invenManger[userid].userCraftToolClient.Find(x => x.CreftType.Equals(craftType));        
        isHaveTool = userCraftTool.Item_ID != 0;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Interact.performed -= OnInteractFishing;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        NetworkIdentity playerIdentity = other.GetComponent<NetworkIdentity>();
        // 만약 해당 플레이어가 로컬 플레이어일 경우에만 상호작용 UI 및 파티클 활성화
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            currentPlayer = other.gameObject;  // 상호작용 중인 플레이어 저장
            InteractAction.SetActive(isHaveTool);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        
        NetworkIdentity playerIdentity = other.GetComponent<NetworkIdentity>();

        // 만약 로컬 플레이어가 포탈 영역을 벗어났을 경우 상호작용 UI 및 파티클 비활성화
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            InteractAction.SetActive(false);            
            currentPlayer = null;  // 플레이어가 나가면 null로 초기화
        }
    }

    private void OnInteractFishing(InputAction.CallbackContext context)
    {
        Debug.Log("여기도 안 되나?");
        if (true == InteractAction.activeSelf)
        {
            Debug.Log("여기는? 돼 ?");
            InteractAction.SetActive(false);
            craft.Action();
        }
    }
}
