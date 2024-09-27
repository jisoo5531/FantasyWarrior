using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour
{
    public static BlacksmithManager Instance { get; private set; }

    /// <summary>
    /// �������� ������ ��� ��ųʸ�
    /// <para>key�� ������ npc�� ID</para>
    /// </summary>
    private Dictionary<int, BlackSmithData> blacksmith_Dict;
    /// <summary>
    /// �� �����ڵ��� � �������� ���� �� �ִ����� ���� ������ ���� ����Ʈ    
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
    /// �ش� ������ npc�� � �������� ���� �� �ִ����� ���� ����Ʈ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="blacksmith_ID"></param>
    /// <returns></returns>
    public List<BlacksmithRecipeData> GetBlacksmithRecipeList(int blacksmith_ID)
    {
        return blacksmithRecipeList.FindAll(x => x.Blacksmith_ID == blacksmith_ID);
    }
    /// <summary>
    /// �ش� ���۰� ����� �������� ���� ��� ��������
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
    /// ��� �������� ���� ��������
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
            // ����
        }
    }
    /// <summary>
    /// �� �����ڵ��� � �������� ���� �� �ִ����� ���� ���� ��������
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
            // ����
        }
    }
    #endregion
}
