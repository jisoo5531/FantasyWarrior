using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // TODO : 임시 레벨업 테스트 버튼
    public Button levelUpButton;

    public static PlayerUIManager Instance { get; private set; }

    public List<Sprite> skillIconList = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        Initialize();
        
    }
    private void Start()
    {        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        string folderName = $"{userStatClient.charClass.ToString()}";
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
