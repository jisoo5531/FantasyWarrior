using System.Data;

public class Questobj_CollectData
{
    public int Quest_ID { get; set; }
    public int Item_ID { get; set; }
    public int CollectAmount { get; set; }

    public Questobj_CollectData(DataRow row) : this
        (
            int.Parse(row["Quest_ID"].ToString()),
            int.Parse(row["Item_ID"].ToString()),
            int.Parse(row["Collect_Count"].ToString())
        )
    { }
    public Questobj_CollectData(int quest_ID, int item_ID, int collectAmount)
    {
        Quest_ID = quest_ID;
        Item_ID = item_ID;
        CollectAmount = collectAmount;
    }
}
