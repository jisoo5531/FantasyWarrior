using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum CharClass
{
    Warrior,
    Archer
}
public class UserData
{
    public string Email { get; set; }
    public string Passwd { get; private set; }
    public string Name { get; set; }
    public CharClass CharClass { get; set; }  
    public int Level { get; set; }
    
    public int UID { get; set; }

    public UserData(DataRow row) : this
        (
            row["email"].ToString(),
            row["pw"].ToString(),
            row["name"].ToString(),                                    
            (CharClass)int.Parse(row["class"].ToString()),
            int.Parse(row["LEVEL"].ToString()),            
            int.Parse(row["uid"].ToString())
        ) { }

    public UserData(string email, string passwd, string name, CharClass charClass, int level, int uID)
    {
        this.Email = email;
        this.Passwd = passwd;
        this.Name = name;
        this.CharClass = charClass;
        this.Level = level;
        this.UID = uID;
    }
}
