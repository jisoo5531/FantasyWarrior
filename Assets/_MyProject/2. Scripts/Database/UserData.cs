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
    public int UID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Passwd { get; private set; }
    public CharClass CharClass { get; set; }  
    public int Level { get; set; }
    public int EXP { get; set; }
    public int Gold { get; set; }
        

    public UserData(DataRow row) : this
        (
            int.Parse(row["user_id"].ToString()),
            row["username"].ToString(),                                    
            row["email"].ToString(),
            row["password_hash"].ToString(),            
            (CharClass)int.Parse(row["class"].ToString()),
            int.Parse(row["level"].ToString()),            
            int.Parse(row["exp"].ToString()),            
            int.Parse(row["gold"].ToString())            
        ) { }

    public UserData(int uID, string name, string email, string passwd, CharClass charClass, int level, int eXP, int gold)
    {
        this.UID = uID;
        this.Name = name;
        this.Email = email;
        this.Passwd = passwd;
        this.CharClass = charClass;
        this.Level = level;
        this.EXP = eXP;
        this.Gold = gold;
    }
}
