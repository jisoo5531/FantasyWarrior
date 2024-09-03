using System;
using UnityEngine;

public static class EventHandler
{
    public static SkillKey skillKey = new();
}
public class SkillKey
{
    private event Action OnKeyChangeSkill_1;
    private event Action OnKeyChangeSkill_2;
    private event Action OnKeyChangeSkill_3;
    private event Action OnKeyChangeSkill_4;

    public void RegisterSkillKeyChange(Action listener, int number)
    {
        switch (number)
        {
            case 1:
                OnKeyChangeSkill_1 += listener;
                break;
            case 2:
                OnKeyChangeSkill_2 += listener;
                break;
            case 3:
                OnKeyChangeSkill_3 += listener;
                break;
            case 4:
                OnKeyChangeSkill_4 += listener;
                break;
            default:
                break;
        }        
    }
    public void UnRegisterHpChange(Action listener, int number)
    {
        switch (number)
        {
            case 1:
                OnKeyChangeSkill_1 -= listener;
                break;
            case 2:
                OnKeyChangeSkill_2 -= listener;
                break;
            case 3:
                OnKeyChangeSkill_3 -= listener;
                break;
            case 4:
                OnKeyChangeSkill_4 -= listener;
                break;
            default:
                break;
        }
    }    
    public void TriggetSkillKeyChange(int number)
    {
        switch (number)
        {
            case 1:
                OnKeyChangeSkill_1?.Invoke();
                break;
            case 2:
                OnKeyChangeSkill_2?.Invoke();
                break;
            case 3:
                OnKeyChangeSkill_3?.Invoke();
                break;
            case 4:
                OnKeyChangeSkill_4?.Invoke();
                break;
            default:
                break;
        }
        
    }
}
