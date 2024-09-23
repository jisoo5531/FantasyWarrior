
using System.Data;

public class Location_NPCData
{
    public int Location_ID { get; set; }
    public int NPC_ID { get; set; }

    public Location_NPCData(DataRow row) : this
        (
            int.Parse(row["Location_ID"].ToString()),
            int.Parse(row["NPC_ID"].ToString())
        )
    { }
    public Location_NPCData(int location_ID, int nPC_ID)
    {
        this.Location_ID = location_ID;
        this.NPC_ID = nPC_ID;
    }
}
