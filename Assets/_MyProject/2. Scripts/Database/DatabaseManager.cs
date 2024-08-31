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
            conn.Open();
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

        if (ExcuteNonQ(cmd))
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

        if (ExcuteNonQ(cmd))
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
    public void Login(string email, string passwd, Action<UserData> successCallback, Action failureCallback)
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


        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM {tableName} WHERE email='{email}' AND pw='{passwd}'";

        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
        DataSet set = new DataSet();

        dataAdapter.Fill(set);

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

        if (ExcuteNonQ(cmd))
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

        if (ExcuteNonQ(cmd))
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

        if (ExcuteNonQ(cmd))
        {
            data.Name = name;            

            data = UpdateData();
            successCallback?.Invoke(data);
        }
    }

    #endregion

    private bool ExcuteNonQ(MySqlCommand cmd)
    {
        int queryCount = cmd.ExecuteNonQuery();
        return queryCount > 0;
    }

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
}
