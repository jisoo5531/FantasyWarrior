using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("¿©±â ¾È µÅ?");
        Application.Quit();
#endif
    }
}
