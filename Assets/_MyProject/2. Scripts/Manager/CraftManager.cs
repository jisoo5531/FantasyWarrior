using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public void Initialize()
    {

    }


}
