using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private int maxHp_Amount = 150;
    private int maxMana_Amount = 50;
    private int STR_Amount = 5;
    private int DEX_Amount = 5;
    private int INT_Amount = 5;
    private int LUK_Amount = 5;
    private int DEF_Amount = 2;

    private void Awake()
    {
        
    }

    private void Start()
    {
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnLevelUpStatChange);
    }

    private void OnDisable()
    {
        EventHandler.playerEvent.UnRegisterPlayerLevelUp(OnLevelUpStatChange);
    }

    private void OnLevelUpStatChange()
    {
        UserStatData stat = DatabaseManager.Instance.userStatData;

        string query =
            $"UPDATE userstats\n" +
            $"SET maxhp={stat.MaxHp + maxHp_Amount}," +
            $"maxmana={stat.MaxMana + maxMana_Amount}," +
            $"str={stat.STR + STR_Amount}," +
            $"dex={stat.DEX + DEX_Amount}," +
            $"Intelligence={stat.INT + INT_Amount}," +
            $"luk={stat.LUK + LUK_Amount}," +
            $"defense={stat.DEF + DEF_Amount};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        DatabaseManager.Instance.GetUserStatDataTest();
        Debug.Log("¿©±â´Ù");
    }
}
