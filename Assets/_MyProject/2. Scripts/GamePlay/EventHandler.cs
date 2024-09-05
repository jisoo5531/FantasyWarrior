using System;
using UnityEngine;

public static class EventHandler
{
    public static SkillKeyEvent skillKey = new();
    public static PlayerEvent playerEvent = new();
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
