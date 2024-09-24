using System.Data;

public class NPC_ShopData
{
    public int NPC_Shop_ID { get; set; }   
    public string Shop_Name { get; set; }
    public int NPC_ID { get; set; }
    public int Location_ID { get; set; }

    public NPC_ShopData(DataRow row) : this
        (
            int.Parse(row["npc_shop_id"].ToString()),
            row["Name"].ToString(),
            int.Parse(row["npc_id"].ToString()),
            int.Parse(row["location_id"].ToString())
        )
    { }

    public NPC_ShopData(int nPC_Shop_ID, string shop_Name, int nPC_ID, int location_ID)
    {
        this.NPC_Shop_ID = nPC_Shop_ID;
        this.Shop_Name = shop_Name;
        this.NPC_ID = nPC_ID;
        this.Location_ID = location_ID;
    }
}
