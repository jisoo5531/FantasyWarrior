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
    /// �÷��̾��� ����ġ�� 100�� �Ǿ��� �� ������
    /// </summary>
    public void OnLevelUp()
    {
        UserStatManager.Instance.LevelUpUpdateStat();
        EventHandler.playerEvent.TriggerPlayerLevelUp();
    }
}
