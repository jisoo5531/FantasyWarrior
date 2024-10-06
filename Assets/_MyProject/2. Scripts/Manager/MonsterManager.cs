using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mirror;

public class MonsterManager : NetworkBehaviour
{
    public static MonsterManager Instance { get; private set; }

    /// <summary>
    /// 몬스터들의 정보를 담아놓은 딕셔너리
    /// <para>key는 몬스터의 ID.</para>
    /// </summary>
    public Dictionary<int, MonsterData> Monster_Dict { get; private set; }

    private void OnEnable()
    {
        
    }
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {        
        GetMonsterDataFromDB();
    }

    /// <summary>
    /// 해당 ID의 몬스터 정보 가져오기
    /// </summary>
    /// <param name="monster_ID"></param>
    /// <returns></returns>
    public MonsterData GetMonsterData(int monster_ID)
    {        
        if (Monster_Dict.TryGetValue(monster_ID, out MonsterData monster))
        {            
            return monster;
        }        
        return null;
    }

    #region DB

    private void GetMonsterDataFromDB()
    {
        Monster_Dict = new Dictionary<int, MonsterData>();
        string query =
            $"SELECT *\n" +
            $"FROM monsters;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Monster_ID"].ToString());
                Monster_Dict.Add(id, new MonsterData(row));                
            }
        }
        else
        {
            //  실패            
        }
    }

    #endregion
}
