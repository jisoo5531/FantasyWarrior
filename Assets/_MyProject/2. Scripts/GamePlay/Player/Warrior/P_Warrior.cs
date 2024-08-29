using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WarriorSkill))]
public class P_Warrior : PlayerController
{
    protected override void PlayerInit()
    {
        base.PlayerInit();
        playerSkill = GetComponent<WarriorSkill>();
    }
}
