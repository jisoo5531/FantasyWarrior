using System.Data;

public class BlackSmithData
{
    public int BlackSmith_ID { get; set; }
    public int Location_ID { get; set; }
    public string Desc { get; set; }

    public BlackSmithData(DataRow row) : this
        (
            int.Parse(row["Blacksmith_ID"].ToString()),
            int.Parse(row["Location_ID"].ToString()),
            row["Blacksmith_ID"].ToString()
        )
    { }
    public BlackSmithData(int blackSmith_ID, int location_ID, string desc)
    {
        BlackSmith_ID = blackSmith_ID;
        Location_ID = location_ID;
        Desc = desc;
    }
}
