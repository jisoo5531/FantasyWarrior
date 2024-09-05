using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static PlayerInputAction inputActions;

    // TODO : 나중에 SkillManager로 따로 관리하기    

    public GameObject goblin;
    public GameObject mummy;
    public GameObject Item;
    private void Awake()
    {
        Instance = this;
        inputActions = new();
    }
    private void OnEnable()
    {        
        inputActions.PlayerActions.Enable();
    }
    private void OnDisable()
    {
        
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(goblin, new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), goblin.transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(mummy, new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), mummy.transform.rotation);
        }
    }


   
}
