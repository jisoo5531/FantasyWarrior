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
    /// ��Ż�� �̿��Ϸ��� �� �� ����
    /// </summary>
    public void ActivePortal()
    {
        EventHandler.sceneEvent.TriggerSceneOut();
        SceneManager.Instance.SetSceneNumber(currentSceneNumber, nextSceneNumber);        
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneNumber);
    }
}
