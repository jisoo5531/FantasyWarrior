using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Status
{
    public TMP_Text LvText;
    public TMP_Text STRText;
    public TMP_Text DEXText;
    public TMP_Text INTText;
    public TMP_Text LUKText;
    public TMP_Text DEFText;
    public TMP_Text ATKText;
    public TMP_Text HPText;
    public TMP_Text MPText;
}
public class UI_StatPanel : MonoBehaviour
{
    public Status statusUI;
    private UserStatClient userStatClient;

    private void Start()
    {
        UserStatManager.Instance.OnLevelUpUpdateStat += SetStat;
        PlayerEquipManager.Instance.OnEquipItem += SetStat;
        PlayerEquipManager.Instance.OnUnEquipItem += SetStat;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetStat;
        SetStat();
    }
    private void OnDestroy()
    {
        UserStatManager.Instance.OnLevelUpUpdateStat -= SetStat;
        PlayerEquipManager.Instance.OnEquipItem -= SetStat;
        PlayerEquipManager.Instance.OnUnEquipItem -= SetStat;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick -= SetStat;
    }

    private void SetStat()
    {
        this.userStatClient = UserStatManager.Instance.userStatClient;

        statusUI.LvText.text = userStatClient.Level.ToString();
        statusUI.ATKText.text = userStatClient.ATK.ToString();
        statusUI.STRText.text = userStatClient.STR.ToString();
        statusUI.DEXText.text = userStatClient.DEX.ToString();
        statusUI.INTText.text = userStatClient.INT.ToString();
        statusUI.LUKText.text = userStatClient.LUK.ToString();
        statusUI.DEFText.text = userStatClient.DEF.ToString();
        statusUI.HPText.text = userStatClient.MaxHP.ToString();
        statusUI.MPText.text = userStatClient.MaxMP.ToString();        
    }    
}
