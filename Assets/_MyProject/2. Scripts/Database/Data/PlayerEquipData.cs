using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerEquipData
{
    public int UID { get; set; }
    public int HeadItem_ID { get; set; }
    public int ArmorItem_ID { get; set; }
    public int GloveItem_ID { get; set; }
    public int BootItem_ID { get; set; }
    public int WeaponItem_ID { get; set; }
    public int Pendant_ID { get; set; }
    public int Ring_ID { get; set; }

    public PlayerEquipData(DataRow row) : this
        (
            int.Parse(row["User_ID"].ToString()),
            int.TryParse(row["HeadItem_ID"]?.ToString(), out int headItemId) ? headItemId : 0,
            int.TryParse(row["ArmorItem_ID"]?.ToString(), out int armorItemId) ? armorItemId : 0,
            int.TryParse(row["GlovesItem_ID"]?.ToString(), out int glovesItemId) ? glovesItemId : 0,
            int.TryParse(row["BootsItem_ID"]?.ToString(), out int bootsItemId) ? bootsItemId : 0,
            int.TryParse(row["WeaponItem_ID"]?.ToString(), out int weaponItemId) ? weaponItemId : 0,
            int.TryParse(row["PendantItem_ID"]?.ToString(), out int pendantItemId) ? pendantItemId : 0,
            int.TryParse(row["RingItem_ID"]?.ToString(), out int ringItemId) ? ringItemId : 0
        )
    { }


    public PlayerEquipData(int uID, int headItem_ID, int armorItem_ID, int gloveItem_ID, int bootItem_ID, int weaponItem_ID, int pendant_ID, int ring_ID)
    {
        this.UID = uID;
        this.HeadItem_ID = headItem_ID;
        this.ArmorItem_ID = armorItem_ID;
        this.GloveItem_ID = gloveItem_ID;
        this.BootItem_ID = bootItem_ID;
        this.WeaponItem_ID = weaponItem_ID;
        this.Pendant_ID = pendant_ID;
        this.Ring_ID = ring_ID;
    }
}
