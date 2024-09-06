using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UserData
{
    public int UID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Passwd { get; private set; }
        

    public UserData(DataRow row) : this
        (
            int.Parse(row["user_id"].ToString()),
            row["username"].ToString(),                                    
            row["email"].ToString(),
            row["password_hash"].ToString()              
        )
    { }

    public UserData(int uID, string name, string email, string passwd)
    {
        this.UID = uID;
        this.Name = name;
        this.Email = email;
        this.Passwd = passwd;
    }
}
