using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CraftRecipeManager : MonoBehaviour
{
    public static CraftRecipeManager Instance { get; private set; }

    /// <summary>
    /// 아이템 제작 레시피를 담는 딕셔너리
    /// <para>key는 레시피의 ID</para>
    /// </summary>
    private Dictionary<int, CraftingRecipeData> craftRecipe_Dict;
    /// <summary>
    /// 아이템 제작에 필요한 재료들을 담는 리스트
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
    /// 해당 레시피의 정보 가져오기 
    /// <para>(레시피로 만들 아이템, 소요 시간)</para>
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
    /// 해당 레시피의 재료 정보 가져오기
    /// </summary>
    /// <param name="recipe_ID"></param>
    /// <returns></returns>
    public List<RecipeMaterialData> GetRecipeMaterialList(int recipe_ID)
    {
        return recipeMaterialList.FindAll(x => x.Recipe_ID == recipe_ID);
    }

    #region DB
    /// <summary>
    /// 아이템 제작 레시피 정보들 가져오기
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
            // 실패
        }
    }
    /// <summary>
    /// 레시피의 재료 정보 가져오기
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
            // 실패
        }
    }

    #endregion
}
