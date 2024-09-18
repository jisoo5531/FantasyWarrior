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
    [Header("��ų ����")]
    public VideoPlayer videoPlayer;
    public List<VideoClip> skillVideoClipList;

    public void Initialize(SkillData skillData)
    {
        videoPlayer.clip = skillVideoClipList[skillData.Skill_Order - 1];
        skillNameText.text = skillData.Skill_Name;
        skillDescText.text = skillData.Skill_Desc;
    }
}
