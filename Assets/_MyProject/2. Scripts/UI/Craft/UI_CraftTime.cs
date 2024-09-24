using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftTime : MonoBehaviour
{
    public Slider progressBar;
    public float actionTime;
    public void Initalize(float actionTime)
    {
        this.actionTime = actionTime;
        progressBar.maxValue = actionTime;
        StartCoroutine(StartTime());
    }
    private IEnumerator StartTime()
    {
        float currentTime = 0f;
        while (true)
        {
            Debug.Log(currentTime);
            if (currentTime >= actionTime)
            {
                break;
            }
            currentTime += Time.deltaTime;
            progressBar.value = currentTime;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
