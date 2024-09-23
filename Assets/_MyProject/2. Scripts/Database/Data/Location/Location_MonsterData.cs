
using System.Data;

public class Location_MonsterData
{
    public int Location_ID { get; set; }
    public int Monster_ID { get; set; }

    public Location_MonsterData(DataRow row) : this
        (
            int.Parse(row["Location_ID"].ToString()),
            int.Parse(row["Monster_ID"].ToString())
        )
    { }
    public Location_MonsterData(int location_ID, int monster_ID)
    {
        Location_ID = location_ID;
        Monster_ID = monster_ID;
    }
}
