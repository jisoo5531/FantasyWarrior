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

    // ������ ���� �г��� ���ȴ� ���¸� ���
    private bool wasShopPanelOpen = false;
    private bool wasCraftPanelOpen = false;

    private void Awake()
    {
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
        }
        Instance = this;        
    }


    private void OnEnable()
    {
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
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
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
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

        // ���� �г��� �ݾ��� �� ���� �Ǵ� ���� �г��� �������� �Ǵ�
        if (!isActive)
        {
            RestoreShopOrCraftPanel();
        }
    }

    // ��� �г��� ���� �� ���� �Ǵ� ���� â�� ����
    private void RestoreShopOrCraftPanel()
    {
        // �ٸ� �г��� ��� ������ ���� ����/���� â ����
        if (!SkillPanel.gameObject.activeSelf &&
            !InventoryPanel.gameObject.activeSelf &&
            !QuestPanel.gameObject.activeSelf &&
            !StatPanel.gameObject.activeSelf)
        {
            if (wasShopPanelOpen)
            {
                Debug.Log("���� ����");
                ShopPanel.gameObject.SetActive(true);
                wasShopPanelOpen = false;  // ���������Ƿ� �ʱ�ȭ
                playerUI.gameObject.SetActive(!ShopPanel.gameObject.activeSelf);
            }
            else if (wasCraftPanelOpen)
            {
                Debug.Log("���� ����");
                CraftPanel.gameObject.SetActive(true);
                wasCraftPanelOpen = false;  // ���������Ƿ� �ʱ�ȭ
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
        // ��� �г��� ������ ���� �÷��̾� UI�� Ȱ��ȭ
        playerUI.gameObject.SetActive(!anyPanelOpen);
    }
    private void CloseOtherPanels(GameObject activePanel)
    {
        if (SkillPanel.gameObject != activePanel) SkillPanel.gameObject.SetActive(false);
        if (InventoryPanel.gameObject != activePanel) InventoryPanel.gameObject.SetActive(false);
        if (QuestPanel.gameObject != activePanel) QuestPanel.gameObject.SetActive(false);
        if (StatPanel.gameObject != activePanel) StatPanel.gameObject.SetActive(false);

        // ���� �Ǵ� ���� â�� ���� �־��ٸ� �ϴ� ������ ���´� ���
        if (ShopPanel.gameObject.activeSelf)
        {
            Debug.Log("�����?");
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
