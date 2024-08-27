using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject goblin;
    void Awake()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(goblin, new Vector3(Random.Range(0, 10), 0, 0), goblin.transform.rotation);
        }
    }

}
