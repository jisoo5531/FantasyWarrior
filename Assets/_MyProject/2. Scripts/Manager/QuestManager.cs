using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    private List<QuestsData> questsDataList;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 퀘스트 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    public List<QuestsData> GetQuestDataFromDB()
    {        
        questsDataList = new List<QuestsData>();
        string query =
            $"SELECT *\n" +
            $"FROM quests\n" +
            $"WHERE quests.Quest_ID=1;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {            
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                questsDataList.Add(new QuestsData(row));
            }
            return questsDataList;
        }
        else
        {
            //  실패
            return null;
        }
    }
}
