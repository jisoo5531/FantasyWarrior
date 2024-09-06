using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum CharClass
{
    None,
    Warrior,
    Archer
}
public class UserStatData
{
    public int UID { get; set; }
    public CharClass CharClass { get; set; }
    public int Level { get; set; }
    public int EXP { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    public int STR { get; set; }
    public int DEX { get; set; }
    public int INT { get; set; }
    public int LUK { get; set; }
    public int DEF { get; set; }
    public int Gold { get; set; }

    public UserStatData(DataRow row) : this
        (
            int.Parse(row["user_id"].ToString()),
            (CharClass)int.Parse(row["class"].ToString()),
            int.Parse(row["level"].ToString()),
            int.Parse(row["exp"].ToString()),
            int.Parse(row["maxhp"].ToString()),
            int.Parse(row["hp"].ToString()),
            int.Parse(row["maxmana"].ToString()),
            int.Parse(row["mana"].ToString()),
            int.Parse(row["str"].ToString()),
            int.Parse(row["dex"].ToString()),
            int.Parse(row["intelligence"].ToString()),
            int.Parse(row["luck"].ToString()),
            int.Parse(row["defense"].ToString()),
            int.Parse(row["gold"].ToString())
        )
    { }

    public UserStatData(int uID, CharClass charClass, int level, int eXP, int maxHp, int hp, int maxMana, int mana, int sTR, int dEX, int iNT, int lUK, int dEF, int gold)
    {
        this.UID = uID;
        this.CharClass = charClass;
        this.Level = level;
        this.EXP = eXP;
        this.MaxHp = maxHp;
        this.Hp = hp;
        this.MaxMana = maxMana;
        this.Mana = mana;
        this.STR = sTR;
        this.DEX = dEX;
        this.INT = iNT;
        this.LUK = lUK;
        this.DEF = dEF;
        this.Gold = gold;
    }
}
