using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCraftTool : MonoBehaviour
{
    public GameObject CutDownTree;
    public GameObject Harvest;

    public void OnCutDownTree()
    {
        CutDownTree.SetActive(true);
    }
    public void OffCutDownTree()
    {
        CutDownTree.SetActive(false);
    }
    public void OnHarvest()
    {
        Harvest.SetActive(true);
    }
    public void OffHarvest()
    {
        Harvest.SetActive(false);
    }
}
