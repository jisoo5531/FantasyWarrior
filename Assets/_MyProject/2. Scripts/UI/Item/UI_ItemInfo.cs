using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ItemInfo : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text ReqLevelText;
    public TMP_Text ATKText;    
    public TMP_Text DEFText;    
    public TMP_Text STRText;    
    public TMP_Text DEXText;    
    public TMP_Text INTText;    
    public TMP_Text LUKText;    
    public TMP_Text HPText;    
    public TMP_Text MPText;

    public void Initialize(int itemID)
    {
        EquipItemData equipItemData = ItemManager.Instance.GetEquipItemFromDB(itemID);
        string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);

        itemImage.sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
        itemNameText.text = itemName;
        ReqLevelText.text = $"REQ LV.: {equipItemData.Require_LV.ToString()}";
        ATKText.text = $"ATK: +{equipItemData.ATK_Boost.ToString()}";
        DEFText.text = $"DEF: +{equipItemData.DEF_Boost.ToString()}";
        STRText.text = $"STR: +{equipItemData.STR_Boost.ToString()}";
        DEXText.text = $"DEX: +{equipItemData.DEX_Boost.ToString()}";
        INTText.text = $"INT: +{equipItemData.INT_Boost.ToString()}";
        LUKText.text = $"LUK: +{equipItemData.LUK_Boost.ToString()}";
        HPText.text = $"HP: +{equipItemData.Hp_Boost.ToString()}";
        MPText.text = $"MP: +{equipItemData.Mp_Boost.ToString()}";        
    }
}
