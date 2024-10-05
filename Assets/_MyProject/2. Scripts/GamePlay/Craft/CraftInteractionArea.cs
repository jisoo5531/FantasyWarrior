using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftInteractionArea : MonoBehaviour
{
    public LayerMask playerLayer;
    /// <summary>
    /// ��ȣ�ۿ� ������
    /// </summary>
    public GameObject InteractAction;

    public CraftType craftType;

    public Craft craft;

    private GameObject currentPlayer;    // ���� ��ȣ�ۿ� ���� �÷��̾�
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
        // TODO : ���߿� �������� �Ȱų� ������ ��� ���� ��, �׶����� üũ���ֵ���        
        
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
        // ���� �ش� �÷��̾ ���� �÷��̾��� ��쿡�� ��ȣ�ۿ� UI �� ��ƼŬ Ȱ��ȭ
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            currentPlayer = other.gameObject;  // ��ȣ�ۿ� ���� �÷��̾� ����
            InteractAction.SetActive(isHaveTool);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        
        NetworkIdentity playerIdentity = other.GetComponent<NetworkIdentity>();

        // ���� ���� �÷��̾ ��Ż ������ ����� ��� ��ȣ�ۿ� UI �� ��ƼŬ ��Ȱ��ȭ
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            InteractAction.SetActive(false);            
            currentPlayer = null;  // �÷��̾ ������ null�� �ʱ�ȭ
        }
    }

    private void OnInteractFishing(InputAction.CallbackContext context)
    {
        Debug.Log("���⵵ �� �ǳ�?");
        if (true == InteractAction.activeSelf)
        {
            Debug.Log("�����? �� ?");
            InteractAction.SetActive(false);
            craft.Action();
        }
    }
}
