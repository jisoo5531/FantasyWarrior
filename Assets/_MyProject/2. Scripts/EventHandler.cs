using System;
using UnityEngine;

public static class EventHandler
{
    public static PlayerAction playerAction = new();
}
public class PlayerAction
{
    public event Action PlayerAttack;

    public void RegisterAttack(Action listener)
    {
        PlayerAttack += listener;
    }

    public void TriggerAttack()
    {
        PlayerAttack?.Invoke();
    }    
}
