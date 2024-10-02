using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MonsterData
{
    public int MonsterID { get; set; }
    public string MonsterName { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int Damage { get; set; }
    public int Defense { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackRange { get; set; }
    public int EXP_Reward { get; set; }
    public int Gold_Reward { get; set; }
    public int Item_Reward { get; set; }

    public MonsterData(DataRow row) : this
        (
            int.Parse(row["monster_id"].ToString()),
            row["monstername"].ToString(),
            int.Parse(row["maxhp"].ToString()),
            int.Parse(row["hp"].ToString()),
            int.Parse(row["damage"].ToString()),
            int.Parse(row["defense"].ToString()),
            float.Parse(row["MoveSpeed"].ToString()),
            float.Parse(row["AttackRange"].ToString()),
            int.Parse(row["EXP_Reward"].ToString()),
            int.Parse(row["Gold_Reward"].ToString()),
            int.Parse(row["Item_Reward"].ToString())
        )
    { }

    public MonsterData(int monsterID, string monsterName, int maxHp, int hp, int damage, int defense, float moveSpeed, float attackRange, int eXP_Reward, int gold_Reward, int item_Reward)
    {
        MonsterID = monsterID;
        MonsterName = monsterName;
        MaxHp = maxHp;
        Hp = hp;
        Damage = damage;
        Defense = defense;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        EXP_Reward = eXP_Reward;
        Gold_Reward = gold_Reward;
        Item_Reward = item_Reward;
    }
}

