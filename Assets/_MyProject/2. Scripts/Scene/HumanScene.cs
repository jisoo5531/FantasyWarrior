using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyScene;
public class HumanScene : MonoBehaviour
{
    public Transform GameStartPos;
    public Transform GoblinToHumanPos;



    private void Start()
    {
        //Debug.Log($"씬 넘버 차이 : {SceneManager.Instance.CurrentMinusNext()}");
        //if (SceneManager.Instance.CurrentMinusNext() == 1)
        //{
        //    GameStart();
        //}
        //else if (SceneManager.Instance.CurrentMinusNext() == -1)
        //{
        //    Debug.Log("여기로 가는거지?");
        //    GoblinToHuman();
        //}
        //SceneManager.Instance.currentSceneNumber = (int)Scene.Human;

        if (SceneManager.Instance.currentSceneNumber == 2)
        {
            GoblinToHuman();
        }
        SceneManager.Instance.currentSceneNumber = 1;
    }

    public void GameStart()
    {
        GameObject.Find("Player").transform.position = GameStartPos.position;
        EventHandler.sceneEvent.TriggerSceneIn();
    }
    public void GoblinToHuman()
    {
        GameObject.Find("Player").transform.position = GoblinToHumanPos.position;
        EventHandler.sceneEvent.TriggerSceneIn();
    }    
}
