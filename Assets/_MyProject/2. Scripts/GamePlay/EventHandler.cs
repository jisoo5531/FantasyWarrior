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
    #region 퀘스트
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

    #region 인벤토리
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

    #region 스탯
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

    #region 장비
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

    #region 스킬
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

    #region 아이템
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

    public void RegisterNPCManagerInit(Action listener)
    {
        NPCManagerInit += listener;
    }
    public void UnRegisterNPCManagerInit(Action listener)
    {
        NPCManagerInit -= listener;
    }
    public void TriggerNPCManagerInit()
    {
        NPCManagerInit?.Invoke();
    }
    #endregion

    #region 몬스터
    private event Action MonsteranagerInit;

    public void RegisterMonsteranagerInit(Action listener)
    {
        MonsteranagerInit += listener;
    }
    public void UnRegisterMonsteranagerInit(Action listener)
    {
        MonsteranagerInit -= listener;
    }
    public void TriggerMonsteranagerInit()
    {
        MonsteranagerInit?.Invoke();
    }
    #endregion
}
