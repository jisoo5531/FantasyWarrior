using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // TODO : �ӽ� ������ �׽�Ʈ ��ư
    public Button levelUpButton;

    public static PlayerUIManager Instance { get; private set; }

    public List<Sprite> skillIconList = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        Initialize();

        levelUpButton.onClick.AddListener(() => { PlayerController.OnLevelUp(); });
    }
    private void Start()
    {
        string folderName = $"{DatabaseManager.Instance.userStatData.CharClass.ToString()}";
        foreach (var item in Resources.LoadAll<Sprite>($"{folderName}_skills"))
        {
            skillIconList.Add(item);
        }
    }

    private void Initialize()
    {
        

        //foreach (var item in Resources.LoadAll<Sprite>("archer_skills"))
        //{
        //    skillIconList.Add(item);
        //}
    }
}
