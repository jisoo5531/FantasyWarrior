using System.Data;

public class QuestObj_TalkData
{
    public int Quest_ID { get; set; }
    public int NPC_ID { get; set; }

    public QuestObj_TalkData(DataRow row) : this
        (
            int.Parse(row["Quest_ID"].ToString()),
            int.Parse(row["NPC_ID"].ToString())
        )
    { }
    public QuestObj_TalkData(int quest_ID, int nPC_ID)
    {
        Quest_ID = quest_ID;
        NPC_ID = nPC_ID;
    }
}
