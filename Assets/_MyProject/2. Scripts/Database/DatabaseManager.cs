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
    private string rootPassword = "1234";   // �׽�Ʈ �ÿ� Ȱ���� �� ������ ���ȿ� ����ϹǷ� ����

    private MySqlConnection conn;           // mySql DB�� ������¸� �����ϴ� ��ü

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
    #region ȸ������

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

    #region ȸ��Ż��

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

    #region �α���

    // �α����� �Ϸ��� �� ��, �α��� ������ ���� ��� �����Ͱ� ���� ���� �� �����Ƿ�,
    // �α����� �Ϸ�Ǿ��� �� ȣ��� �Լ��� �Ķ���ͷ� �Բ� �޾��ֵ��� ��.
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

        // Dispose�� ���� �ʰ� using �����θ�
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
            // �α��� ����(email�� pw ���� ���ÿ� ��ġ�ϴ� ���� ������.)
            DataRow row = set.Tables[0].Rows[0];

            UserData data = new UserData(row);

            successCallback?.Invoke(data);
        }
        else
        {
            // �α��� ����
            failureCallback?.Invoke();
        }
    }

    #endregion

    #region ������

    public void LevelUP(UserData data, Action successCallback)
    {
        int level = data.Level;
        int nextLevel = level + 1;

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE {tableName} SET level={nextLevel} WHERE uid={data.UID}";

        if (ExcuteNonQ(cmd))
        {
            // ������ ���������� ����� ���
            data.Level = nextLevel;
            successCallback?.Invoke();
        }
        else
        {
            // ���� ���� ����
        }
    }

    #endregion

    #region ���� ����

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

    #region ���� ����

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
