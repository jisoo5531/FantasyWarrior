using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    private int userId;

    /// <summary>
    /// 상점 데이터들을 담는 딕셔너리
    /// <para>key는 상점의 ID</para>
    /// </summary>
    private Dictionary<int, NPC_ShopData> Shop_Dict;
    /// <summary>
    /// 위와 같은 상점 데이터들을 담는 딕셔너리
    /// <para>key는 npc의 ID</para>
    /// </summary>
    private Dictionary<int, NPC_ShopData> npc_Shop_Dict;
    /// <summary>
    /// 각 상점에서 어떤 물품을 파는지에 대한 정보를 담는 딕셔너리
    /// <para>key는 상점 아이템의 ID</para>
    /// </summary>
    private Dictionary<int, NPC_Shop_Item_Data> Shop_Item_Dict;
    /// <summary>
    /// 각 상점에서 어떤 물품을 파는지에 대한 정보를 담는 리스트
    /// </summary>
    private List<NPC_Shop_Item_Data> Shop_Item_List;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        GetShopDataFromDB();
        GetShopItemListFromDB();
    }
    /// <summary>
    /// 해당 npc가 어떤 상점을 운영하는지의 정보를 가져오기
    /// </summary>
    /// <param name="npcShop_Id"></param>
    /// <returns></returns>
    public NPC_ShopData GetShopData_NpcShop(int npcId)
    {
        if (npc_Shop_Dict.TryGetValue(npcId, out NPC_ShopData shop))
        {
            return shop;
        }
        return null;
    }
    /// <summary>
    /// 해당 상점 아이템ID의 상점에서 파는 아이템 정보 가져오기
    /// </summary>
    /// <param name="shopItemID"></param>
    /// <returns></returns>
    public NPC_Shop_Item_Data GetShopItem(int shopItemID)
    {
        if (Shop_Item_Dict.TryGetValue(shopItemID, out NPC_Shop_Item_Data shopItem))
        {
            return shopItem;
        }
        return null;
    }
    /// <summary>
    /// 해당 npc 상점의 아이템 리스트 가져오기
    /// </summary>
    /// <param name="npcID"></param>
    /// <returns></returns>
    public List<NPC_Shop_Item_Data> GetShopItemList(int npcID)
    {
        NPC_ShopData shop = GetShopData_NpcShop(npcID);
        Debug.Log($"얘 null? : {shop == null}");
        return Shop_Item_List.FindAll(x => x.NPC_Shop_ID == shop.NPC_Shop_ID);
    }
    /// <summary>
    /// 상점에서 아이템을 살 때 호출
    /// </summary>
    public void BuyItem(int userID, int shopItemID, int amount, Action BuySuccess, Action BuyFailure)
    {
        if (Shop_Item_Dict.TryGetValue(shopItemID, out NPC_Shop_Item_Data shopItem))
        {
            if (false == UserStatManager.Instance.UseGold(shopItem.Price * amount))
            {
                // 돈이 부족해 못 사는 경우엔                
                BuyFailure?.Invoke();
            }
            else
            {
                Debug.Log("아이템 성공적으로 샀다.");
                ItemData item = ItemManager.Instance.GetItemData(shopItem.Item_ID);
                
                GameManager.Instance.invenManager[userID].GetItem(userID, item, amount);                
                BuySuccess?.Invoke();
            }
            //shopItem.Price
        }
        else
        {
            Debug.Log("없어?");
        }
    }
    /// <summary>
    /// 상점에 아이템을 팔 때 호출
    /// </summary>
    public void SellItem(ItemData item, int amount, Action successSell)
    {
        UserStatManager.Instance.GetGold(item.SellPrice * amount);

        GameManager.Instance.invenManager[this.userId].SubtractItem(item, amount);        

        successSell?.Invoke();
    }

    #region DB

    /// <summary>
    /// 모든 상점 데이터 가져오기    
    /// </summary>
    private void GetShopDataFromDB()
    {
        Shop_Dict = new Dictionary<int, NPC_ShopData>();
        npc_Shop_Dict = new Dictionary<int, NPC_ShopData>();
        string query =
            $"SELECT *\n" +
            $"FROM npc_shop;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int shopID = int.Parse(row["npc_shop_id"].ToString());
                int npcID = int.Parse(row["npc_id"].ToString());
                Shop_Dict.Add(shopID, new NPC_ShopData(row));
                npc_Shop_Dict.Add(npcID, new NPC_ShopData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    /// <summary>
    /// 각 상점에서 어떤 물품을 파는지에 대한 정보 가져오기
    /// </summary>
    private void GetShopItemListFromDB()
    {
        Shop_Item_Dict = new Dictionary<int, NPC_Shop_Item_Data>();
        Shop_Item_List = new List<NPC_Shop_Item_Data>();        

        string query =
            $"SELECT *\n" +
            $"FROM npc_shop_items;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int shopItemID = int.Parse(row["npc_shop_item_id"].ToString());                
                Shop_Item_Dict.Add(shopItemID, new NPC_Shop_Item_Data(row));
                Shop_Item_List.Add(new NPC_Shop_Item_Data(row));
            }
        }
        else
        {
            //  실패
        }
    }

    #endregion
}
