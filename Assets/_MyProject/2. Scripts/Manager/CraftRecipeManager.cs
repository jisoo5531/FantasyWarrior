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
    /// ������ ���� �����Ǹ� ��� ��ųʸ�
    /// <para>key�� ������� �������� ID</para>
    /// </summary>
    private Dictionary<int, CraftingRecipeData> craftRecipeKeyItemID_Dict;
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
    /// �ش� ������ �Ǵ� ������� ��� ������ ������ ���� �������� 
    /// <para>(�����Ƿ� ���� ������, �ҿ� �ð�)</para>
    /// </summary>
    /// <param name="recipe_ID"></param>
    /// <returns></returns>
    public CraftingRecipeData GetRecipeData(int recipe_ID = 0, int item_ID = 0)
    {
        if (craftRecipe_Dict.TryGetValue(recipe_ID, out CraftingRecipeData R_Recipe))
        {            
            return R_Recipe;
        }
        else if (craftRecipeKeyItemID_Dict.TryGetValue(item_ID, out CraftingRecipeData I_Recipe))
        {
            return I_Recipe;
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
        craftRecipeKeyItemID_Dict = new Dictionary<int, CraftingRecipeData>();
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
                int itemID = int.Parse(row["Crafted_Item_ID"].ToString());
                craftRecipe_Dict.Add(id, new CraftingRecipeData(row));
                craftRecipeKeyItemID_Dict.Add(itemID, new CraftingRecipeData(row));
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
