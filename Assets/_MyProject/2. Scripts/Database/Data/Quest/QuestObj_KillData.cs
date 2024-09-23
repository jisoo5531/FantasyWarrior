using System.Data;

public class QuestObj_KillData
{
    public int Quest_ID { get; set; }
    public int Monster_ID { get; set; }
    public int KillAmount { get; set; }

    public QuestObj_KillData(DataRow row) : this
        (
            int.Parse(row["Quest_ID"].ToString()),
            int.Parse(row["Monster_ID"].ToString()),
            int.Parse(row["Kill_Count"].ToString())
        )
    { }
    public QuestObj_KillData(int quest_ID, int monster_ID, int killAmount)
    {
        Quest_ID = quest_ID;
        Monster_ID = monster_ID;
        KillAmount = killAmount;
    }
}
