using System.Data;

public class BlacksmithRecipeData
{
    public int Blacksmith_ID { get; set; }
    public int Recipe_ID { get; set; }
    public int Cost { get; set; }

    public BlacksmithRecipeData(DataRow row) : this
        (
            int.Parse(row["Blacksmith_ID"].ToString()),
            int.Parse(row["Recipe_ID"].ToString()),
            int.Parse(row["Cost"].ToString())
        )
    { }
    public BlacksmithRecipeData(int blacksmith_ID, int recipe_ID, int cost)
    {
        Blacksmith_ID = blacksmith_ID;
        Recipe_ID = recipe_ID;
        Cost = cost;
    }
}
