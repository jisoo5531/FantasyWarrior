using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySqlConnector;
using System;
using System.Security.Cryptography;
using System.Data;
using System.Text;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string serverIP = "127.0.0.1";
    private string portHum = "9333";
    private string dbName = "test";
    private string tableName = "user";
    private string rootPassword = "1234";   // 테스트 시에 활용할 수 있지만 보안에 취약하므로 주의

    private MySqlConnection conn;           // mySql DB와 연결상태를 유지하는 객체

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {        
        DBConnect();
    }
    public void DBConnect()
    {        
        try
        {
            string config = $"server={serverIP};port={portHum};database={dbName};uid=root;pwd={rootPassword};charset=utf8;";

            conn = new MySqlConnection(config);            
            Debug.Log("Connect Success");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);            
        }        
    }
    #region 회원가입

    public void SignUP(string email, string passwd, Action successCallback, Action failureCallback)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"INSERT INTO {tableName}(email, pw, LEVEL, class) VALUES('{email}', '{passwd}', {0}, {0})";

        if (ExcuteNonQuery(cmd))
        {
            successCallback?.Invoke();
        }
        else
        {
            failureCallback?.Invoke();
        }
    }

    #endregion

    #region 회원탈퇴

    public void SignOut(string email, string passwd, Action successCallback, Action failureCallback)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"DELETE FROM {tableName} WHERE email='{email}'";

        if (ExcuteNonQuery(cmd))
        {
            successCallback?.Invoke();
        }
        else
        {
            failureCallback?.Invoke();
        }
    }

    #endregion

    #region 로그인

    // 로그인을 하려고 할 때, 로그인 쿼리를 날린 즉시 데이터가 오지 않을 수 있으므로,
    // 로그인이 완료되었을 때 호출될 함수를 파라미터로 함께 받아주도록 함.
    public void Login(string email, string passwd, Action<UserData> successCallback = null, Action failureCallback = null)
    {
        string pwHash = "";

        SHA256 sha256 = SHA256.Create();
        byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
        foreach (byte b in hashArray)
        {
            pwHash += $"{b:X2}";
        }
        sha256.Dispose();

        print(pwHash);

        // Dispose를 쓰지 않고 using 문으로만
        //using (SHA256 sha256 = SHA256.Create())
        //{
        //    byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd));
        //    foreach (byte b in hashArray)
        //    {
        //        pwHash += $"{b:X2}";
        //    }
        //}
        string query = $"SELECT * FROM {tableName} WHERE email='{email}' AND password_hash='{passwd}'";
        Dictionary<string, object> where = new Dictionary<string, object>
        {
            { "email", email },
            { "password_hash", passwd }
        };
        DataSet set = OnSelectRequest(tableName, where);

        bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

        if (isLoginSuccess)
        {
            // 로그인 성공(email과 pw 값이 동시에 일치하는 행이 존재함.)
            DataRow row = set.Tables[0].Rows[0];

            UserData data = new UserData(row);

            successCallback?.Invoke(data);
        }
        else
        {
            // 로그인 실패
            failureCallback?.Invoke();
        }
    }

    #endregion

    #region 레벨업

    public void LevelUP(UserData data, Action successCallback)
    {
        int level = data.Level;
        int nextLevel = level + 1;

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE {tableName} SET level={nextLevel} WHERE uid={data.UID}";

        if (ExcuteNonQuery(cmd))
        {
            // 쿼리가 정상적으로 실행된 경우
            data.Level = nextLevel;
            successCallback?.Invoke();
        }
        else
        {
            // 쿼리 수행 실패
        }
    }

    #endregion

    #region 직업 선택

    public void ChangeCharClass(UserData data, int classNum, Action<UserData> successCallback)
    {
        CharClass changedClass = (CharClass)classNum;

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE {tableName} SET class={classNum} WHERE uid={data.UID}";
        
        if (ExcuteNonQuery(cmd))
        {
            data.CharClass = changedClass;

            data = UpdateData();
            successCallback?.Invoke(data);
        }
    }

    #endregion

    #region 정보 수정

    public void EditInfo(UserData data, string name, string profile_Text, Action<UserData> successCallback, Action failureCallback)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE {tableName} SET name='{name}', profile_text='{profile_Text}'";

        if (ExcuteNonQuery(cmd))
        {
            data.Name = name;            

            data = UpdateData();
            successCallback?.Invoke(data);
        }
    }

    #endregion

    

    private UserData UpdateData()
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM {tableName}";

        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
        DataSet set = new DataSet();

        bool isExist = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

        if (isExist)
        {
            DataRow row = set.Tables[0].Rows[0];

            UserData data = new UserData(row);

            return data;
        }
        else
        {
            return null;
        }
    }

    public bool OnInsertOrUpdateRequest(string query)
    {
        try
        {
            conn.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;            

            if (ExcuteNonQuery(cmd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;            
        }
    }
    public DataSet OnSelectRequest(string tableName, Dictionary<string, object> where)
    {
        try
        {
            conn.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            if (where.Count > 0)
            {
                // 조건들을 리스트에 담고, AND로 연결
                List<string> conditions = new List<string>();

                foreach (var kvp in where)
                {
                    conditions.Add($"{kvp.Key}=@{kvp.Key}");
                    cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }

                // 조건들을 AND로 연결하여 WHERE 절 생성
                string whereClause = string.Join(" AND ", conditions);
                cmd.CommandText = $"SELECT * FROM {tableName} WHERE {whereClause}";
            }
            else
            {
                cmd.CommandText = $"SELECT * FROM {tableName}";
            }                                    

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            return set;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }
    /// <summary>
    /// MySqlCommand.ExcuteNonQuery() 했을 때 결과가 있는지
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    private bool ExcuteNonQuery(MySqlCommand cmd)
    {
        int queryCount = cmd.ExecuteNonQuery();
        conn.Close();
        return queryCount > 0;
    }

    private void OnApplicationQuit()
    {
        conn.Close();
    }
}
