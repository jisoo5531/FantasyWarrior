using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StatPanel : MonoBehaviour
{
    public Status statusUI;
    private UserStatData userStatData;

    private void Awake()
    {
        
    }

    private void Start()
    {
        EventHandler.playerEvent.RegisterPlayerLevelUp(SetStat);
        userStatData = DatabaseManager.Instance.userStatData;

        SetStat();
    }

    private void SetStat()
    {
        userStatData = DatabaseManager.Instance.userStatData;
        statusUI.STRText.text = userStatData.STR.ToString();
        statusUI.DEXText.text = userStatData.DEX.ToString();
        statusUI.INTText.text = userStatData.INT.ToString();
        statusUI.LUKText.text = userStatData.LUK.ToString();
    }
}
[System.Serializable]
public class Status
{    
    public TMP_Text STRText;
    public TMP_Text DEXText;
    public TMP_Text INTText;
    public TMP_Text LUKText;
}