using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    

    private void Awake()
    {
        
    }

    private void Start()
    {        
    }

    private void OnDisable()
    {        
    }

    /// <summary>
    /// 플레이어의 경험치가 100이 되었을 때 레벨업
    /// </summary>
    public void OnLevelUp()
    {
        UserStatManager.Instance.LevelUpUpdateStat();
        EventHandler.playerEvent.TriggerPlayerLevelUp();
    }
}
