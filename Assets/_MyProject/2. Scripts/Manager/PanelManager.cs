using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [Header("PlayerUI")]
    public PlayerUI playerUI;
    [Header("Panel")]
    public UI_InventoryPanel InventoryPanel;
    public UI_SkillPanel SkillPanel;
    public UI_QuestPanel QuestPanel;
    public UI_StatPanel StatPanel;
    public UI_ShopPanel ShopPanel;
    public UI_CraftPanel CraftPanel;

    // 상점과 제작 패널이 열렸던 상태를 기록
    private bool wasShopPanelOpen = false;
    private bool wasCraftPanelOpen = false;

    private void Awake()
    {
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        Instance = this;        
    }


    private void OnEnable()
    {
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        PlayerSkill.OnKeyBindInit += SkillPanel.SkillPanelInit;
        var playerActions = GameManager.inputActions.PlayerActions;
        playerActions.UI_Skill.performed += OnSkillUI;
        playerActions.UI_Inventory.performed += OnInventoryUI;
        playerActions.UI_Quest.performed += OnQuestUI;
        playerActions.UI_Status.performed += OnStatUI;
    }

    private void OnDisable()
    {
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        PlayerSkill.OnKeyBindInit -= SkillPanel.SkillPanelInit;
        var playerActions = GameManager.inputActions.PlayerActions;
        playerActions.UI_Skill.performed -= OnSkillUI;
        playerActions.UI_Inventory.performed -= OnInventoryUI;
        playerActions.UI_Quest.performed -= OnQuestUI;
        playerActions.UI_Status.performed -= OnStatUI;
    }

    private void TogglePanel(GameObject panel)
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);
        
        UpdateUIState();
        CloseOtherPanels(panel);

        // 현재 패널을 닫았을 때 상점 또는 제작 패널을 복원할지 판단
        if (!isActive)
        {
            RestoreShopOrCraftPanel();
        }
    }

    // 모든 패널이 닫힐 때 상점 또는 제작 창을 복원
    private void RestoreShopOrCraftPanel()
    {
        // 다른 패널이 모두 닫혔을 때만 상점/제작 창 복원
        if (!SkillPanel.gameObject.activeSelf &&
            !InventoryPanel.gameObject.activeSelf &&
            !QuestPanel.gameObject.activeSelf &&
            !StatPanel.gameObject.activeSelf)
        {
            if (wasShopPanelOpen)
            {
                Debug.Log("상점 복구");
                ShopPanel.gameObject.SetActive(true);
                wasShopPanelOpen = false;  // 복구했으므로 초기화
                playerUI.gameObject.SetActive(!ShopPanel.gameObject.activeSelf);
            }
            else if (wasCraftPanelOpen)
            {
                Debug.Log("제작 복구");
                CraftPanel.gameObject.SetActive(true);
                wasCraftPanelOpen = false;  // 복구했으므로 초기화
                playerUI.gameObject.SetActive(!CraftPanel.gameObject.activeSelf);
            }
        }
    }
    private void UpdateUIState()
    {
        bool anyPanelOpen = SkillPanel.gameObject.activeSelf ||
                            InventoryPanel.gameObject.activeSelf ||
                            QuestPanel.gameObject.activeSelf ||
                            StatPanel.gameObject.activeSelf ||
                            ShopPanel.gameObject.activeSelf ||
                            CraftPanel.gameObject.activeSelf;
        Debug.Log(CraftPanel.gameObject.activeSelf);
        // 모든 패널이 닫혔을 때만 플레이어 UI를 활성화
        playerUI.gameObject.SetActive(!anyPanelOpen);
    }
    private void CloseOtherPanels(GameObject activePanel)
    {
        if (SkillPanel.gameObject != activePanel) SkillPanel.gameObject.SetActive(false);
        if (InventoryPanel.gameObject != activePanel) InventoryPanel.gameObject.SetActive(false);
        if (QuestPanel.gameObject != activePanel) QuestPanel.gameObject.SetActive(false);
        if (StatPanel.gameObject != activePanel) StatPanel.gameObject.SetActive(false);

        // 상점 또는 제작 창이 열려 있었다면 일단 닫지만 상태는 기억
        if (ShopPanel.gameObject.activeSelf)
        {
            Debug.Log("여기야?");
            wasShopPanelOpen = true;
            ShopPanel.gameObject.SetActive(false);
        }
        if (CraftPanel.gameObject.activeSelf)
        {
            wasCraftPanelOpen = true;
            CraftPanel.gameObject.SetActive(false);
        }
    }
    private void OnSkillUI(InputAction.CallbackContext context)
    {
        TogglePanel(SkillPanel.gameObject);                
    }

    private void OnInventoryUI(InputAction.CallbackContext context)
    {
        TogglePanel(InventoryPanel.gameObject);        
    }

    private void OnQuestUI(InputAction.CallbackContext context)
    {
        TogglePanel(QuestPanel.gameObject);        
    }

    private void OnStatUI(InputAction.CallbackContext context)
    {
        TogglePanel(StatPanel.gameObject);        
    }
}
