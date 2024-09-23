using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationManger : MonoBehaviour
{
    public static LocationManger Instance { get; private set; }
    public int currentRegionID { get; private set; }
    public List<NPC> NPC_List { get; private set; }

    // TODO : �ӽ�
    public int anna = 1;

    private void Awake()
    {
        Instance = this;
    }
    public void Initialize()
    {
        // TODO : �ӽ�
        currentRegionID = 1;
        AddCurrentScene_NPC();
    }   
    /// <summary>
    /// ���� ��. ��, ���� �ʿ� �ִ� NPC�� ã�ƿͼ� ����Ʈ�� �ֱ�
    /// </summary>
    private void AddCurrentScene_NPC()
    {        
        NPC_List = FindObjectsOfType<NPC>().ToList();        
    }
}
