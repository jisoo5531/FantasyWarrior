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

    private MySqlConnection conn;           // mySql DB와 연결상태를 유지하는 객체

    private string serverIP = "52.79.56.41";
    private string portHum = "3306";
    private string dbName = "game";
    private string tableName = "users";
    private string rootPassword = "1234";   // 테스트 시에 활용할 수 있지만 보안에 취약하므로 주의

    // TODO : 테스트용 임시 저장    
    public UserData userData { get; private set; }
    public UserStatData userStatData { get; private set; }

    private void Awake()
    {
        Instance = this;
        DBConnect();
        GetUserDataTest();
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
    /// <summary>
    /// TODO : 임시
    /// </summary>
    public void GetUserDataTest()
    {
        string query =
            $"SELECT *\n" +
            $"FROM users\n" +
            $"WHERE user_id = 1;";

        DataSet dataSet = OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            userData = new UserData(row);
            //foreach (DataRow row in dataSet.Tables[0].Rows)
            //{
            //    SkillData data = new SkillData(row);               
            //}
        }
        else
        {
            //  실패
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

        // TODO : 로그인 query문 바꾸기
        DataSet set = OnSelectRequest(tableName);

        bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

        if (isLoginSuccess)
        {
            // 로그인 성공(email과 pw 값이 동시에 일치하는 행이 존재함.)
            DataRow row = set.Tables[0].Rows[0];

            userData = new UserData(row);

            successCallback?.Invoke(userData);
        }
        else
        {
            // 로그인 실패
            failureCallback?.Invoke();
        }
    }

    #endregion

    #region 레벨업

    public void LevelUP(Action successCallback = null)
    {
        conn.Open();
        int level = userStatData.Level;
        int nextLevel = level + 1;

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE userstats SET level={nextLevel} WHERE user_id={userData.UID}";

        if (ExcuteNonQuery(cmd))
        {
            conn.Close();
            // 쿼리가 정상적으로 실행된 경우
            userStatData.Level = nextLevel;
            successCallback?.Invoke();
        }
        else
        {
            // 쿼리 수행 실패
        }


    }

    #endregion

    #region 직업 선택

    //public void ChangeCharClass(UserData data, int classNum, Action<UserData> successCallback)
    //{
    //    CharClass changedClass = (CharClass)classNum;

    //    MySqlCommand cmd = new MySqlCommand();
    //    cmd.Connection = conn;
    //    cmd.CommandText = $"UPDATE {tableName} SET class={classNum} WHERE uid={data.UID}";

    //    if (ExcuteNonQuery(cmd))
    //    {
    //        data.CharClass = changedClass;

    //        data = UpdateData();
    //        successCallback?.Invoke(data);
    //    }
    //}

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
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;            
        }
    }
    /// <summary>
    /// Select Query를 위한 메서드.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="whereQuery">조건문 쿼리, 없다면 null</param>
    /// <returns></returns>
    public DataSet OnSelectRequest(string query, string tableName = null)
    {
        try
        {
            conn.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;                                

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            conn.Close();

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
        return queryCount > 0;
    }

    private void OnApplicationQuit()
    {
        conn.Close();
    }
}
