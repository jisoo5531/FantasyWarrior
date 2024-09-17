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
    public int MaxExp { get; set; }
    public int EXP { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    public int ATK { get; set; }
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
            int.Parse(row["maxexp"].ToString()),
            int.Parse(row["exp"].ToString()),
            int.Parse(row["maxhp"].ToString()),
            int.Parse(row["hp"].ToString()),
            int.Parse(row["maxmana"].ToString()),
            int.Parse(row["mana"].ToString()),
            int.Parse(row["atk"].ToString()),
            int.Parse(row["str"].ToString()),
            int.Parse(row["dex"].ToString()),
            int.Parse(row["intelligence"].ToString()),
            int.Parse(row["luk"].ToString()),
            int.Parse(row["defense"].ToString()),
            int.Parse(row["gold"].ToString())
        )
    { }

    public UserStatData(int uID, CharClass charClass, int level, int maxExp, int eXP, int maxHp, int hp, int maxMana, int mana, int aTK, int sTR, int dEX, int iNT, int lUK, int dEF, int gold)
    {
        this.UID = uID;
        this.CharClass = charClass;
        this.Level = level;
        this.MaxExp = maxExp;
        this.EXP = eXP;
        this.MaxHp = maxHp;
        this.Hp = hp;
        this.MaxMana = maxMana;
        this.Mana = mana;
        this.ATK = aTK;
        this.STR = sTR;
        this.DEX = dEX;
        this.INT = iNT;
        this.LUK = lUK;
        this.DEF = dEF;
        this.Gold = gold;
    }
}
/// <summary>
/// <para>���� ���Ȱ��� ������ �ִ� Ŭ����</para>
/// <para>������ ������ �� �� �̰ɷ� �̿��Ѵ�.</para>
/// ��� ����, ������ / ������
/// </summary>
public class UserStatClient
{
    #region ����
    /// <summary>
    /// ���������� ������ ���Ȱ�.
    /// </summary>
    public class LevelUpStat
    {
        public int MaxExpAmount = 2137;
        public int MaxhpAmount = 500;
        public int MaxmpAmount = 150;
        public int ATKAmount = 5;
        public int STRAmount = 5;
        public int DEXAmount = 5;
        public int INTAmount = 5;
        public int LUKAmount = 5;
        public int DEFAmount = 5;
    }
    /// <summary>
    /// ���������� ������ ���Ȱ�
    /// </summary>
    public LevelUpStat levelUpStat = new();

    #region ���� ����
    private int orgLv = 1;
    private int orgMaxExp = 100;
    private int orgStr = 5;
    private int orgDex = 5;    
    private int orgInt = 5;
    private int orgLuk = 5;
    private int orgAtk = 5;
    private int orgDef = 5;
    private int orgMaxHP = 1000;
    private int orgMaxMP = 500;
    /// <summary>
    /// ���� ����
    /// </summary>
    public int O_Lv => orgLv;
    /// <summary>
    /// ���� STR ���Ȱ�.
    /// </summary>
    public int O_STR => orgStr;
    /// <summary>
    /// ���� �ִ� ����ġ
    /// </summary>
    public int O_MaxExp => orgMaxExp;
    /// <summary>
    /// ���� DEX ���Ȱ�.
    /// </summary>
    public int O_DEX => orgDex;
    /// <summary>
    /// ���� INT ���Ȱ�.
    /// </summary>
    public int O_INT => orgInt;
    /// <summary>
    /// ���� LUK ���Ȱ�.
    /// </summary>
    public int O_LUK => orgLuk;
    /// <summary>
    /// ���� ATK ���Ȱ�.
    /// </summary>
    public int O_ATK => orgAtk;
    /// <summary>
    /// ���� DEF ���Ȱ�.
    /// </summary>
    public int O_DEF => orgDef;
    /// <summary>
    /// ���� MaxHp ���Ȱ�.
    /// </summary>
    public int O_MaxHP => orgMaxHP;
    /// <summary>
    /// ���� MaxMana ���Ȱ�.
    /// </summary>
    public int O_MaxMP => orgMaxMP;
    #endregion

    public CharClass charClass;
    public int Level = 1;
    public int MaxExp = 100;
    public int Exp = 0;
    public int STR = 5;
    public int DEX = 5;
    public int INT = 5;
    public int LUK = 5;
    public int ATK = 5;
    public int DEF = 5;
    public int MaxHP = 1000;
    public int HP = 1000;
    public int MaxMP = 500;
    public int MP = 500;

    #endregion

    public UserStatClient(UserStatData userStat)
    {
        this.charClass = userStat.CharClass;
        this.Level = userStat.Level;        
        this.Exp = userStat.EXP;
        this.MaxExp = orgMaxExp + (levelUpStat.MaxExpAmount * (Level - 1));
        this.ATK = orgAtk + (levelUpStat.ATKAmount * (Level - 1));
        this.STR = orgStr + (levelUpStat.STRAmount * (Level - 1));
        this.DEX = orgDex + (levelUpStat.DEXAmount * (Level - 1));
        this.INT = orgInt + (levelUpStat.INTAmount * (Level - 1));
        this.LUK = orgLuk + (levelUpStat.LUKAmount * (Level - 1));
        this.DEF = orgDef + (levelUpStat.DEFAmount * (Level - 1));
        this.MaxHP = orgMaxHP + (levelUpStat.MaxhpAmount * (Level - 1));
        this.MaxMP = orgMaxMP + (levelUpStat.MaxmpAmount * (Level - 1));
        this.HP = this.MaxHP;
        this.MP = this.MaxMP;
    }

    #region ���� ������Ʈ �Լ�
    public int UpdateLv(int amount)
    {
        return Level += amount;
    }
    public int UpdateExp(int amount)
    {
        return Exp += amount;
    }    
    public int UpdateMaxExp(int amount)
    {
        return MaxExp += amount;
    }
    public int UpdateSTR(int amount)
    {
        return STR += amount;
    }
    public int UpdateDEX(int amount)
    {
        return DEX += amount;
    }
    public int UpdateINT(int amount)
    {
        return INT += amount;
    }
    public int UpdateLUK(int amount)
    {
        return LUK += amount;
    }
    public int UpdateATK(int amount)
    {
        return ATK += amount;
    }
    public int UpdateDEF(int amount)
    {
        return DEF += amount;
    }
    public int UpdateMaxHP(int amount)
    {
        return MaxHP += amount;
    }
    public int UpdateHP(int amount)
    {
        return HP += amount;
    }
    public int UpdateMaxMP(int amount)
    {
        return MaxMP += amount;
    }
    public int UpdateMP(int amount)
    {
        return MP += amount;
    }
    #endregion
}
