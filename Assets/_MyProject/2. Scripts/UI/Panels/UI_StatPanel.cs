using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Status
{
    public TMP_Text STRText;
    public TMP_Text DEXText;
    public TMP_Text INTText;
    public TMP_Text LUKText;
}
public class UI_StatPanel : MonoBehaviour
{
    public Status statusUI;
    private UserStatData userStatData;
    private PlayerEquipData playerEquipData;
    
    private List<EquipItemData> playerEquipList;

    private int STRAmount = 0;
    private int DEXAmount = 0;
    private int INTAmount = 0;
    private int LUKAmount = 0;
    private int ATKAmount = 0;
    private int DEFAmount = 0;
    private int HPAmount = 0;
    private int MPAmount = 0;

    private void Awake()
    {
        
    }

    private void Start()
    {
        EventHandler.playerEvent.RegisterPlayerLevelUp(SetStat);
        PlayerEquipManager.Instance.OnEquipItem += SetStat;
        PlayerEquipManager.Instance.OnUnEquipItem += SetStat;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetStat;

        SetStat();
    }

    private void SetStat()
    {
        this.userStatData = UserStatManager.Instance.GetUserStatDataFromDB();
        this.playerEquipData = PlayerEquipManager.Instance.GetPlayerEquipFromDB();

        statusUI.STRText.text = userStatData.STR.ToString();
        statusUI.DEXText.text = userStatData.DEX.ToString();
        statusUI.INTText.text = userStatData.INT.ToString();
        statusUI.LUKText.text = userStatData.LUK.ToString();
    }    
}
