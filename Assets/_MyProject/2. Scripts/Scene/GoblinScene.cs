using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyScene;
public class GoblinScene : MonoBehaviour
{
    public GameObject player;
    public GameObject UI;
    public Transform playerSpawnPos;
    private void Start()
    {
        SceneManager.Instance.currentSceneNumber = (int)Scene.Goblin;
        //Invoke("PlayerPosInit", 2f);
        PlayerPosInit();

        SoundManager.Instance.PlayBGM("GoblinBGM");
    }
    private void PlayerPosInit()
    {
        //GameObject player = GameObject.Find("Player");
        GameObject playerObj = Instantiate(player, playerSpawnPos.position, Quaternion.identity);
        Instantiate(UI);
        FindObjectOfType<SetCamera>().Initialize(playerObj);
        //player.transform.position = playerSpawnPos.transform.position;
        EventHandler.sceneEvent.TriggerSceneIn();
    }    
}
