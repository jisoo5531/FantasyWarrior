using System.Data;

public class NPC_Shop_Item_Data
{
    public int NPC_Shop_Item_Order { get; set; }
    public int NPC_Shop_ID { get; set; }
    public int Item_ID { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }

    public NPC_Shop_Item_Data(DataRow row) : this
        (
            int.Parse(row["npc_shop_item_id"].ToString()),
            int.Parse(row["npc_shop_id"].ToString()),
            int.Parse(row["item_id"].ToString()),
            int.Parse(row["price"].ToString()),
            int.Parse(row["stock"].ToString())
        )
    { }
    public NPC_Shop_Item_Data(int nPC_Shop_Item_Order, int nPC_Shop_ID, int item_ID, int price, int stock)
    {
        this.NPC_Shop_Item_Order = nPC_Shop_Item_Order;
        this.NPC_Shop_ID = nPC_Shop_ID;
        this.Item_ID = item_ID;
        this.Price = price;
        this.Stock = stock;
    }
}
