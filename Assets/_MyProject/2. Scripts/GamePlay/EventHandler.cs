using System;
using UnityEngine;

public static class EventHandler
{
    public static GameStartEvent gameStartEvent = new();
    public static SkillKeyEvent skillKey = new();
    public static PlayerEvent playerEvent = new();
    public static ManagerEvent managerEvent = new();
    public static QuestNavEvent questNavEvent = new();
    public static SceneEvent sceneEvent = new();
    public static MonsterEvent monsterEvent = new();
}

public class GameStartEvent
{
    private event Action OnGameStart;

    public void RegisterGameStart(Action listener)
    {
        OnGameStart += listener;
    }
    public void UnRegisterGameStart(Action listener)
    {
        OnGameStart -= listener;
    }
    public void TriggerGameStart()
    {
        OnGameStart?.Invoke();
    }
}

public class PlayerEvent
{
    #region 레벨업
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
    #endregion

    #region 플레이어 입장
    private event Action<int> OnPlayerEnter;
    private event Action OnPlayerEnterNo;

    public void RegisterPlayerEnter(Action<int> listener)
    {
        OnPlayerEnter += listener;
    }
    public void RegisterPlayerEnter(Action listener)
    {
        OnPlayerEnterNo += listener;
    }
    public void UnRegisterPlayerEnter(Action<int> listener)
    {
        OnPlayerEnter -= listener;
    }
    public void TriggerPlayerEnter(int userid)
    {
        OnPlayerEnter?.Invoke(userid);
        OnPlayerEnterNo?.Invoke();
    }
    #endregion



}
public class MonsterEvent
{
    private event Action OnMonsterCreate;

    public void RegisterMonsterCreate(Action listener)
    {
        OnMonsterCreate += listener;
    }
    public void UnRegisterMonsterCreate(Action listener)
    {
        OnMonsterCreate -= listener;
    }
    public void TriggerMonsterCreate()
    {
        OnMonsterCreate?.Invoke();
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
public class QuestNavEvent
{
    private event Action<QuestData> OnQuestNav;
    public void RegisterQuestNav(Action<QuestData> listener)
    {
        OnQuestNav += listener;
    }
    public void UnRegisterQuestNav(Action<QuestData> listener)
    {
        OnQuestNav -= listener;
    }
    public void TriggerQuestNav(QuestData quest)
    {
        OnQuestNav?.Invoke(quest);
    }
}
public class SceneEvent
{
    #region 씬 아웃
    private event Action OnSceneChangeOut;    
    public void RegisterSceneOut(Action listener)
    {
        OnSceneChangeOut += listener;
    }
    public void UnRegisterSceneOut(Action listener)
    {
        OnSceneChangeOut -= listener;
    }
    public void TriggerSceneOut()
    {
        OnSceneChangeOut?.Invoke();
    }
    #endregion
    #region 씬 인
    private event Action OnSceneChangeIn;    
    public void RegisterSceneIn(Action listener)
    {
        OnSceneChangeIn += listener;
    }
    public void UnRegisterSceneIn(Action listener)
    {
        OnSceneChangeIn -= listener;
    }
    public void TriggerSceneIn()
    {
        OnSceneChangeIn?.Invoke();
    }
    #endregion
}