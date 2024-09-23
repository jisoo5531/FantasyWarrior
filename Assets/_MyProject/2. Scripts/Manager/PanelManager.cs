using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [Header("PlayerUI")]
    public PlayerUI playerUI;
    [Header("Panel")]
    public UI_InventoryPanel InventoryPanel;
    public UI_SkillPanel SkillPanel;
    public UI_QuestPanel QuestPanel;
    public UI_StatPanel StatPanel;

    private void Awake()
    {
        //EventHandler.managerEvent.RegisterQuestManagerInit(QuestPanel.QuestPanelInit);        
        //EventHandler.managerEvent.RegisterStatManagerInit(StatPanel.StatPanelInit);
        PlayerSkill.OnKeyBindInit += SkillPanel.SkillPanelInit;        
    }

    private void OnEnable()
    {
        GameManager.inputActions.PlayerActions.UI_Skill.performed += OnSkill_UI;
        GameManager.inputActions.PlayerActions.UI_Inventory.performed += OnInventory_UI;
        GameManager.inputActions.PlayerActions.UI_Quest.performed += OnQuest_UI;
        GameManager.inputActions.PlayerActions.UI_Status.performed += OnStat_UI;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.UI_Skill.performed -= OnSkill_UI;
        GameManager.inputActions.PlayerActions.UI_Skill.performed -= OnInventory_UI;
        GameManager.inputActions.PlayerActions.UI_Quest.performed -= OnQuest_UI;
        GameManager.inputActions.PlayerActions.UI_Status.performed -= OnStat_UI;
    }

    private void OnSkill_UI(InputAction.CallbackContext context)
    {
        SkillPanel.gameObject.SetActive(!SkillPanel.gameObject.activeSelf);
        playerUI.gameObject.SetActive(!SkillPanel.gameObject.activeSelf);
        InventoryPanel.gameObject.SetActive(false);
        QuestPanel.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
    }
    private void OnInventory_UI(InputAction.CallbackContext context)
    {
        InventoryPanel.gameObject.SetActive(!InventoryPanel.gameObject.activeSelf);
        playerUI.gameObject.SetActive(!InventoryPanel.gameObject.activeSelf);
        SkillPanel.gameObject.SetActive(false);
        QuestPanel.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
    }
    private void OnQuest_UI(InputAction.CallbackContext context)
    {
        QuestPanel.gameObject.SetActive(!QuestPanel.gameObject.activeSelf);
        playerUI.gameObject.SetActive(!QuestPanel.gameObject.activeSelf);
        SkillPanel.gameObject.SetActive(false);
        InventoryPanel.gameObject.SetActive(false);
        StatPanel.gameObject.SetActive(false);
    }
    private void OnStat_UI(InputAction.CallbackContext context)
    {
        StatPanel.gameObject.SetActive(!StatPanel.gameObject.activeSelf);
        playerUI.gameObject.SetActive(!StatPanel.gameObject.activeSelf);
        SkillPanel.gameObject.SetActive(false);
        InventoryPanel.gameObject.SetActive(false);
        QuestPanel.gameObject.SetActive(false);
    }
}
