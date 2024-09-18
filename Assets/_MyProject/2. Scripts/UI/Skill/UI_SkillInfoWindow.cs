using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class UI_SkillInfoWindow : MonoBehaviour
{
    [Header("��ų �̸�")]
    public TMP_Text skillNameText;
    [Header("��ų ����")]
    public TMP_Text skillDescText;
    [Header("��ų �� ����")]
    public GameObject skillInfoContent;
    public TMP_Text currentSkillInfoTextPrefab;
    public TMP_Text nextSkillInfoTextPrefab;
    [Header("��ų ����")]
    public VideoPlayer videoPlayer;
    public List<VideoClip> skillVideoClipList;

    private SkillData SkillData;

    public void Initialize(SkillData skillData)
    {
        this.SkillData = skillData;
        videoPlayer.clip = skillVideoClipList[skillData.Skill_Order - 1];
        skillNameText.text = skillData.Skill_Name;
        skillDescText.text = skillData.Skill_Desc;
        SetSkillInfoText();
    }
    /// <summary>
    /// ���� ��ų ���� �� ������ ���� ���� ��ų ������ ����
    /// </summary>
    private void SetSkillInfoText()
    {
        ContentClear(skillInfoContent);
        float Multiplier = 0;
        #region ���� ����
        string skillInfo = this.SkillData.Skill_Info;        
        Multiplier = this.SkillData.Multiplier + ((this.SkillData.Level - 1) * this.SkillData.Multi_Amount);        
        int HitCount = this.SkillData.HitCount;

        skillInfo = skillInfo.Replace("{Multiplier}", (Multiplier * 100f).ToString());
        skillInfo = skillInfo.Replace("{HitCount}", HitCount.ToString());
        currentSkillInfoTextPrefab.GetComponent<TMP_Text>().text =
            $"[���緹�� {SkillData.Level}]\n" +
            skillInfo;
        Instantiate(currentSkillInfoTextPrefab, skillInfoContent.transform);
        #endregion

        if (SkillData.Level == SkillData.MaxLevel)
        {
            return;
        }

        #region ��������
        skillInfo = this.SkillData.Skill_Info;        
        Multiplier = this.SkillData.Multiplier + (this.SkillData.Level * this.SkillData.Multi_Amount);
        skillInfo = skillInfo.Replace("{Multiplier}", (Multiplier * 100f).ToString());
        skillInfo = skillInfo.Replace("{HitCount}", HitCount.ToString());
        nextSkillInfoTextPrefab.GetComponent<TMP_Text>().text =
                $"[�������� {SkillData.Level + 1}]\n" +
                skillInfo;
        Instantiate(nextSkillInfoTextPrefab, skillInfoContent.transform);
        #endregion
    }
    private void ContentClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
