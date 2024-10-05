using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterSpawner : NetworkBehaviour
{
    public LayerMask playerLayer;
    public GameObject monsterPrefab;

    [Server]
    public void SpawnMonster(Transform transform)
    {
        Debug.Log("���� ���� �߳�?");
        GameObject monster = Instantiate(monsterPrefab, transform);
        NetworkServer.Spawn(monster);

        // ��� Ŭ���̾�Ʈ�� ���͸� �����϶�� RPC ȣ��
        RpcSpawnMonster(transform.position, transform.rotation);
    }
    [ClientRpc]
    public void RpcSpawnMonster(Vector3 position, Quaternion rotation)
    {
        Debug.LogError("���� �����Ұž�");
        Instantiate(monsterPrefab, position, rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("���� �Դ�!");
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        SpawnMonster(transform);
    }
}
