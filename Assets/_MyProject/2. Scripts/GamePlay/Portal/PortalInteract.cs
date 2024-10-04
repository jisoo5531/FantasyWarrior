using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PortalInteract : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject InteractAction;    // ��ȣ�ۿ� UI
    public GameObject portalParticle;    // ��Ż ��ƼŬ

    private GameObject currentPlayer;    // ���� ��ȣ�ۿ� ���� �÷��̾�

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;

        // �÷��̾ �����ϸ� �ش� �÷��̾��� ��Ʈ��ũ ID�� Ȯ��
        NetworkIdentity playerIdentity = other.GetComponent<NetworkIdentity>();

        // ���� �ش� �÷��̾ ���� �÷��̾��� ��쿡�� ��ȣ�ۿ� UI �� ��ƼŬ Ȱ��ȭ
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            portalParticle.SetActive(true);
            InteractAction.SetActive(true);
            currentPlayer = other.gameObject;  // ��ȣ�ۿ� ���� �÷��̾� ����
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
            portalParticle.SetActive(false);
            currentPlayer = null;  // �÷��̾ ������ null�� �ʱ�ȭ
        }
    }

    // ��Ż ��ȣ�ۿ� �Լ�: ���� �÷��̾ ��ȣ�ۿ� ����
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (currentPlayer != null && InteractAction.activeSelf)
        {
            // ��Ż ��ȣ�ۿ� ���� ����
            transform.parent.GetComponent<Portal>().ActivePortal();

            // ��ȣ�ۿ� UI ����
            InteractAction.SetActive(false);
        }
    }
}
