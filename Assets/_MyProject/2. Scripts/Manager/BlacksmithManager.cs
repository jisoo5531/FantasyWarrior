using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour
{
    public static BlacksmithManager Instance { get; private set; }

    /// <summary>
    /// 제작자의 정보를 담는 딕셔너리
    /// <para>key는 제작자 npc의 ID</para>
    /// </summary>
    private Dictionary<int, BlackSmithData> blacksmith_Dict;
    /// <summary>
    /// 각 제작자들이 어떤 아이템을 만들 수 있는지에 대한 정보를 담은 리스트    
    /// </summary>
    private List<BlacksmithRecipeData> blacksmithRecipeList;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        GetBlacksmithFromDB();
        GetBlacksmithRecipeFromDB();
    }

    /// <summary>
    /// 해당 제작자 npc가 어떤 아이템을 만들 수 있는지에 대한 리스트를 반환하는 메서드
    /// </summary>
    /// <param name="blacksmith_ID"></param>
    /// <returns></returns>
    public List<BlacksmithRecipeData> GetBlacksmithRecipeList(int blacksmith_ID)
    {
        return blacksmithRecipeList.FindAll(x => x.Blacksmith_ID == blacksmith_ID);
    }
    /// <summary>
    /// 해당 제작가 만드는 아이템의 제작 비용 가져오기
    /// </summary>
    /// <param name="blacksmith_ID"></param>
    /// <param name="recipe_Id"></param>
    /// <returns></returns>
    public int GetCraftItemCost(int blacksmith_ID, int recipe_Id)
    {
        return blacksmithRecipeList.Find(x => x.Blacksmith_ID == blacksmith_ID && x.Recipe_ID == recipe_Id).Cost;
    }

    #region DB

    /// <summary>
    /// 모든 제작자의 정보 가져오기
    /// </summary>
    private void GetBlacksmithFromDB()
    {
        blacksmith_Dict = new Dictionary<int, BlackSmithData>();
        string query =
            $"SELECT *\n" +
            $"FROM blacksmiths;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Blacksmith_ID"].ToString());
                blacksmith_Dict.Add(id, new BlackSmithData(row));
            }
        }
        else
        {
            // 실패
        }
    }
    /// <summary>
    /// 각 제작자들이 어떤 아이템을 만들 수 있는지에 대한 정보 가져오기
    /// </summary>
    private void GetBlacksmithRecipeFromDB()
    {
        blacksmithRecipeList = new List<BlacksmithRecipeData>();
        string query =
            $"SELECT *\n" +
            $"FROM blacksmithrecipe;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                blacksmithRecipeList.Add(new BlacksmithRecipeData(row));
            }
        }
        else
        {
            // 실패
        }
    }
    #endregion
}
