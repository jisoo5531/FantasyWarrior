using System;
using UnityEngine;

public static class EventHandler
{
    public static SkillKey skillKey = new();
}
public class SkillKey
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
