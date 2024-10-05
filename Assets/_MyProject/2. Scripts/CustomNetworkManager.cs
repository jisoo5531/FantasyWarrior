using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{    
    public override void OnStartServer()
    {
        if (MonsterManager.Instance == null)
        {
            // MonsterManager 오브젝트를 동적으로 생성
            GameObject monsterManagerObject = new GameObject("MonsterManager");
            MonsterManager monsterManager = monsterManagerObject.AddComponent<MonsterManager>();
            monsterManagerObject.AddComponent<NetworkIdentity>();

            // 초기화 호출
            monsterManager.Initialize();
        }
        else
        {
            MonsterManager.Instance.Initialize();
        }
    }
}
