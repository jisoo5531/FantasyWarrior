using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CraftRecipeManager : MonoBehaviour
{
    public static CraftRecipeManager Instance { get; private set; }

    /// <summary>
    /// ������ ���� �����Ǹ� ��� ��ųʸ�
    /// <para>key�� �������� ID</para>
    /// </summary>
    private Dictionary<int, CraftingRecipeData> craftRecipe_Dict;
    /// <summary>
    /// ������ ���ۿ� �ʿ��� ������ ��� ����Ʈ
    /// </summary>
    private List<RecipeMaterialData> recipeMaterialList;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        GetRecipeFromDB();
        GetRecipeMaterials();
    }

    /// <summary>
    /// �ش� �������� ���� �������� 
    /// <para>(�����Ƿ� ���� ������, �ҿ� �ð�)</para>
    /// </summary>
    /// <param name="recipe_ID"></param>
    /// <returns></returns>
    public CraftingRecipeData GetRecipeData(int recipe_ID)
    {
        if (craftRecipe_Dict.TryGetValue(recipe_ID, out CraftingRecipeData recipe))
        {            
            return recipe;
        }
        return null;
    }
    /// <summary>
    /// �ش� �������� ��� ���� ��������
    /// </summary>
    /// <param name="recipe_ID"></param>
    /// <returns></returns>
    public List<RecipeMaterialData> GetRecipeMaterialList(int recipe_ID)
    {
        return recipeMaterialList.FindAll(x => x.Recipe_ID == recipe_ID);
    }

    #region DB
    /// <summary>
    /// ������ ���� ������ ������ ��������
    /// </summary>
    private void GetRecipeFromDB()
    {
        craftRecipe_Dict = new Dictionary<int, CraftingRecipeData>();
        string query =
            $"SELECT *\n" +
            $"FROM craftingrecipes;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Recipe_ID"].ToString());
                craftRecipe_Dict.Add(id, new CraftingRecipeData(row));
            }            
        }
        else
        {
            // ����
        }
    }
    /// <summary>
    /// �������� ��� ���� ��������
    /// </summary>
    private void GetRecipeMaterials()
    {
        recipeMaterialList = new List<RecipeMaterialData>();
        string query =
            $"SELECT *\n" +
            $"FROM recipematerials;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                recipeMaterialList.Add(new RecipeMaterialData(row));
            }
        }
        else
        {
            // ����
        }
    }

    #endregion
}
