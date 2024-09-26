using System.Data;
/// <summary>
/// 레시피의 재료 정보를 담고 있는 데이터
/// </summary>
public class RecipeMaterialData
{
    public int Recipe_ID { get; set; }
    public int M_Item_ID { get; set; }
    public int M_Quantity { get; set; }

    public RecipeMaterialData(DataRow row) : this
        (
            int.Parse(row["Recipe_ID"].ToString()),
            int.Parse(row["Material_Item_ID"].ToString()),
            int.Parse(row["Quantity"].ToString())
        )
    { }
    public RecipeMaterialData(int recipe_ID, int item_ID, int quantity)
    {
        this.Recipe_ID = recipe_ID;
        this.M_Item_ID = item_ID;
        this.M_Quantity = quantity;
    }
}
