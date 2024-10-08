using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyScene;
public class HumanScene : MonoBehaviour
{
    public Transform GameStartPos;
    public Transform GoblinToHumanPos;

    public GameObject player;
    public GameObject UI;
    SetCamera setCamera;
    private void Start()
    {
        setCamera = FindObjectOfType<SetCamera>();

        if (SceneManager.Instance.currentSceneNumber == 0)
        {
            GameStart();
        }
        else if (SceneManager.Instance.currentSceneNumber == 2)
        {
            GoblinToHuman();
        }
        SceneManager.Instance.currentSceneNumber = 1;
    }

    public void GameStart()
    {
        //GameObject.Find("Player").transform.position = GameStartPos.position;
        GameObject playerObj = Instantiate(player, GameStartPos.position, Quaternion.identity);
        Instantiate(UI);
        setCamera.Initialize(playerObj);
        EventHandler.sceneEvent.TriggerSceneIn();
    }
    public void GoblinToHuman()
    {
        GameObject playerObj = Instantiate(player, GoblinToHumanPos.position, Quaternion.identity);
        Instantiate(UI);
        setCamera.Initialize(playerObj);
        //GameObject.Find("Player").transform.position = GoblinToHumanPos.position;
        EventHandler.sceneEvent.TriggerSceneIn();
    }    
}
