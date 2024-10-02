using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyScene;
public class GoblinScene : MonoBehaviour
{
    public GameObject playerSpawnPos;
    private void Start()
    {
        SceneManager.Instance.currentSceneNumber = (int)Scene.Goblin;
        //Invoke("PlayerPosInit", 2f);
        PlayerPosInit();
    }
    private void PlayerPosInit()
    {        
        GameObject player = GameObject.Find("Player");
        
        player.transform.position = playerSpawnPos.transform.position;
        EventHandler.sceneEvent.TriggerSceneIn();
    }
    private void Update()
    {        
    }
}
