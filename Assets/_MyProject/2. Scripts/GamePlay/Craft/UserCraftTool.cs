using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCraftTool : MonoBehaviour
{
    public GameObject CutDownTree;

    public void OnCutDownTree()
    {
        CutDownTree.SetActive(true);
    }
    public void OffCutDownTree()
    {
        CutDownTree.SetActive(false);
    }
}
