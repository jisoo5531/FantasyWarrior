using System.Data;
/// <summary>
/// ���� �����Ǹ� ��� �ִ� ������
/// </summary>
public class CraftingRecipeData
{
    public int Recipe_ID { get; set; }
    /// <summary>
    /// ������ ������ ID
    /// </summary>
    public int CraftItem_ID { get; set; }
    public float CraftTime { get; set; }

    public CraftingRecipeData(DataRow row) : this
        (
            int.Parse(row["Recipe_ID"].ToString()),
            int.Parse(row["Crafted_Item_ID"].ToString()),
            float.Parse(row["Crafting_Time"].ToString())
        )
    { }
    public CraftingRecipeData(int recipe_ID, int craftItem_ID, float craftTime)
    {
        this.Recipe_ID = recipe_ID;
        this.CraftItem_ID = craftItem_ID;
        this.CraftTime = craftTime;
    }
}
