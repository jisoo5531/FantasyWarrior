using System;
using UnityEngine;

public static class EventHandler
{
    public static SkillKeyEvent skillKey = new();
    public static PlayerEvent playerEvent = new();
    public static ManagerEvent managerEvent = new();
}
public class PlayerEvent
{
    private event Action OnPlayerLevelUp;

    public void RegisterPlayerLevelUp(Action listener)
    {
        OnPlayerLevelUp += listener;
    }
    public void UnRegisterPlayerLevelUp(Action listener)
    {
        OnPlayerLevelUp -= listener;
    }
    public void TriggerPlayerLevelUp()
    {
        OnPlayerLevelUp?.Invoke();
    }
}
public class SkillKeyEvent
{
    private event Action OnKeyChangeSkill;

    public void RegisterSkillKeyChange(Action listener)
    {
        OnKeyChangeSkill += listener;
    }
    public void UnRegisterHpChange(Action listener, int number)
    {

        OnKeyChangeSkill -= listener;

    }
    public void TriggetSkillKeyChange()
    {
        OnKeyChangeSkill?.Invoke();
    }
}
public class ManagerEvent
{
    #region ����Ʈ
    private event Action QuestManagerInit;
    public void RegisterQuestManagerInit(Action listener)
    {
        QuestManagerInit += listener;
    }
    public void UnRegisterQuestManagerInit(Action listener)
    {
        QuestManagerInit -= listener;
    }
    public void TriggerQuestManagerInit()
    {
        QuestManagerInit?.Invoke();
    }
    #endregion

    #region �κ��丮
    private event Action InventoryManagerInit;

    public void RegisterInventoryManagerInit(Action listener)
    {
        InventoryManagerInit += listener;
    }
    public void UnRegisterInventoryManagerInit(Action listener)
    {
        InventoryManagerInit -= listener;
    }
    public void TriggerInventoryManagerInit()
    {
        InventoryManagerInit?.Invoke();
    }
    #endregion

    #region ����
    private event Action StatManagerInit;

    public void RegisterStatManagerInit(Action listener)
    {
        StatManagerInit += listener;
    }
    public void UnRegisterStatManagerInit(Action listener)
    {
        StatManagerInit -= listener;
    }
    public void TriggerStatManagerInit()
    {
        StatManagerInit?.Invoke();
    }
    #endregion

    #region ���
    private event Action EquipManagerInit;

    public void RegisterEquipManagerInit(Action listener)
    {
        EquipManagerInit += listener;
    }
    public void UnRegisterEquipManagerInit(Action listener)
    {
        EquipManagerInit -= listener;
    }
    public void TriggerEquipManagerInit()
    {
        EquipManagerInit?.Invoke();
    }
    #endregion

    #region ��ų
    private event Action SkillManagerInit;

    public void RegisterSkillManagerInit(Action listener)
    {
        SkillManagerInit += listener;
    }
    public void UnRegisterSkillManagerInit(Action listener)
    {
        SkillManagerInit -= listener;
    }
    public void TriggerSkillManagerInit()
    {
        SkillManagerInit?.Invoke();
    }
    #endregion

    #region ������
    private event Action ItemManagerInit;

    public void RegisterItemManagerInit(Action listener)
    {
        ItemManagerInit += listener;
    }
    public void UnRegisterItemManagerInit(Action listener)
    {
        ItemManagerInit -= listener;
    }
    public void TriggerItemManagerInit()
    {
        ItemManagerInit?.Invoke();
    }
    #endregion

    #region NPC
    private event Action NPCManagerInit;

    public void RegisterNPCManagerInitInit(Action listener)
    {
        NPCManagerInit += listener;
    }
    public void UnRegisterNPCManagerInitInit(Action listener)
    {
        NPCManagerInit -= listener;
    }
    public void TriggerNPCManagerInitInit()
    {
        NPCManagerInit?.Invoke();
    }
    #endregion
}
