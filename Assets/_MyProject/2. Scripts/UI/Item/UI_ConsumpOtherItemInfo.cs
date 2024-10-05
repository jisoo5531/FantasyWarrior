using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ConsumpOtherItemInfo : UI_ItemInfo
{    
    public TMP_Text descText;

    public override void Initialize(int userId, int itemID)
    {
        base.Initialize(userId, itemID);
        descText.text = itemData.Item_Description;
    }
}
