

using System.Data;

public class LocationData
{
    public int Location_ID { get; set; }
    public string Name { get; set; }

    public LocationData(DataRow row) : this
        (
            int.Parse(row["Location_ID"].ToString()),
            row["Name"].ToString()
        )
    { }
    public LocationData(int location_ID, string name)
    {
        Location_ID = location_ID;
        Name = name;
    }
}
