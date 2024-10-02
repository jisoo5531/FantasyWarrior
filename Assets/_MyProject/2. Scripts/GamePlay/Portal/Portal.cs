using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyScene;
public class Portal : MonoBehaviour
{
    public int currentSceneNumber;
    public int nextSceneNumber;

    /// <summary>
    /// 포탈을 이용하려고 할 때 실행
    /// </summary>
    public void ActivePortal()
    {
        EventHandler.sceneEvent.TriggerSceneOut();
        SceneManager.Instance.SetSceneNumber(currentSceneNumber, nextSceneNumber);        
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneNumber);
    }
}
