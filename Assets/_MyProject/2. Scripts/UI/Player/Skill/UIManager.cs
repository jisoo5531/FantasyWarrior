using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // TODO : 임시 레벨업 테스트 버튼
    public Button levelUpButton;

    public static UIManager Instance { get; private set; }

    public List<Sprite> skillIconList = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        Initialize();

        levelUpButton.onClick.AddListener(() => { PlayerController.OnLevelUp(); });
    }

    private void Initialize()
    {
        foreach (var item in Resources.LoadAll<Sprite>("warrior_skills"))
        {
            skillIconList.Add(item);
        }

        //foreach (var item in Resources.LoadAll<Sprite>("archer_skills"))
        //{
        //    skillIconList.Add(item);
        //}
    }
}
