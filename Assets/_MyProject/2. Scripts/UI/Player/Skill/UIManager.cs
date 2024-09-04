using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public List<Sprite> skillIconList = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        foreach (var item in Resources.LoadAll<Sprite>("warrior_skills"))
        {
            skillIconList.Add(item);
        }
    }
}
