using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationManger : MonoBehaviour
{
    public static LocationManger Instance { get; private set; }
    public int currentRegionID { get; private set; }
    public List<NPC> NPC_List { get; private set; }

    // TODO : 임시
    public int anna = 1;

    private void Awake()
    {
        Instance = this;
    }
    public void Initialize()
    {
        // TODO : 임시
        currentRegionID = 1;
        AddCurrentScene_NPC();
    }   
    /// <summary>
    /// 현재 씬. 즉, 현재 맵에 있는 NPC들 찾아와서 리스트에 넣기
    /// </summary>
    private void AddCurrentScene_NPC()
    {        
        NPC_List = FindObjectsOfType<NPC>().ToList();        
    }
}
