using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 장비 아이템의 부위
/// </summary>
public enum Equip_Type
{
    Head = 0,
    Armor = 1,
    Glove = 2,
    Boot = 3,
    Weapon = 4,
    Pendant = 5,
    Ring = 6    
}
/// <summary>
/// 인벤토리에 있는 타입이 장비 아이템의 데이터
/// </summary>
public class EquipItemData
{
    public int Equip_ID { get; set; }
    public int Item_ID { get; set; }
    /// <summary>
    /// 장비 아이템의 부위
    /// </summary>
    public Equip_Type Equip_Type { get; set; }
    public int Require_LV { get; set; }
    public int ATK_Boost { get; set; }
    public int DEF_Boost { get; set; }
    public int STR_Boost { get; set; }
    public int DEX_Boost { get; set; }
    public int INT_Boost { get; set; }
    public int LUK_Boost { get; set; }
    public int Hp_Boost { get; set; }
    public int Mp_Boost { get; set; }

    public EquipItemData(DataRow row) : this
        (
            int.Parse(row["equipment_id"].ToString()),
            int.Parse(row["item_id"].ToString()),
            (Equip_Type)System.Enum.Parse(typeof(Equip_Type), row["equipment_type"].ToString()),            
            int.Parse(row["Required_Level"].ToString()),
            int.Parse(row["Attack_Boost"].ToString()),
            int.Parse(row["Defense_Boost"].ToString()),
            int.Parse(row["STR_Boost"].ToString()),
            int.Parse(row["DEX_Boost"].ToString()),
            int.Parse(row["INT_Boost"].ToString()),
            int.Parse(row["LUK_Boost"].ToString()),
            int.Parse(row["Hp_Boost"].ToString()),
            int.Parse(row["Mana_Boost"].ToString())
        )
    { }
    public EquipItemData(int equip_ID, int item_ID, Equip_Type equip_Type, int require_LV, int aTK_Boost, int dEF_Boost, int sTR_Boost, int dEX_Boost, int iNT_Boost, int lUK_Boost, int hp_Boost, int mp_Boost)
    {
        this.Equip_ID = equip_ID;
        this.Item_ID = item_ID;
        this.Equip_Type = equip_Type;
        this.Require_LV = require_LV;
        this.ATK_Boost = aTK_Boost;
        this.DEF_Boost = dEF_Boost;
        this.STR_Boost = sTR_Boost;
        this.DEX_Boost = dEX_Boost;
        this.INT_Boost = iNT_Boost;
        this.LUK_Boost = lUK_Boost;
        this.Hp_Boost = hp_Boost;
        this.Mp_Boost = mp_Boost;
    }
}
