using System;
using UnityEngine;

public static class EventHandler
{
    public static ActionEvent actionEvent = new();
}
public class ActionEvent
{
    private event Action<int> OnHpChange;
    private event Action OnDeath;

    #region HPChange
    public void RegisterHpChange(Action<int> listener)
    {
        OnHpChange += listener;
    }
    public void UnRegisterHpChange(Action<int> listener)
    {
        OnHpChange -= listener;
    }    
    public void TriggerHpChange(int damage)
    {
        OnHpChange?.Invoke(damage);
    }
    #endregion

    #region Death
    public void RegisterDeath(Action listener)
    {
        OnDeath += listener;
    }
    public void UnRegisterDeath(Action listener)
    {
        OnDeath -= listener;
    }
    public void TriggetDeath()
    {
        OnDeath?.Invoke();
    }
    #endregion
}
