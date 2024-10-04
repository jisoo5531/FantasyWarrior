using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PortalInteract : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject InteractAction;    // 상호작용 UI
    public GameObject portalParticle;    // 포탈 파티클

    private GameObject currentPlayer;    // 현재 상호작용 중인 플레이어

    private void Awake()
    {
        GameManager.inputActions.PlayerActions.Interact.performed += OnInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;

        // 플레이어가 접근하면 해당 플레이어의 네트워크 ID를 확인
        NetworkIdentity playerIdentity = other.GetComponent<NetworkIdentity>();

        // 만약 해당 플레이어가 로컬 플레이어일 경우에만 상호작용 UI 및 파티클 활성화
        if (playerIdentity != null && playerIdentity.isLocalPlayer)
        {
            portalParticle.SetActive(true);
            InteractAction.SetActive(true);
            currentPlayer = other.gameObject;  // 상호작용 중인 플레이어 저장
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
            portalParticle.SetActive(false);
            currentPlayer = null;  // 플레이어가 나가면 null로 초기화
        }
    }

    // 포탈 상호작용 함수: 로컬 플레이어만 상호작용 가능
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (currentPlayer != null && InteractAction.activeSelf)
        {
            // 포탈 상호작용 로직 실행
            transform.parent.GetComponent<Portal>().ActivePortal();

            // 상호작용 UI 숨김
            InteractAction.SetActive(false);
        }
    }
}
