using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{    
    public override void OnStartServer()
    {
        if (MonsterManager.Instance == null)
        {
            // MonsterManager ������Ʈ�� �������� ����
            GameObject monsterManagerObject = new GameObject("MonsterManager");
            MonsterManager monsterManager = monsterManagerObject.AddComponent<MonsterManager>();
            monsterManagerObject.AddComponent<NetworkIdentity>();

            // �ʱ�ȭ ȣ��
            monsterManager.Initialize();
        }
        else
        {
            MonsterManager.Instance.Initialize();
        }
    }
}
