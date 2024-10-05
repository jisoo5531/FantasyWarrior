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
        Debug.Log("서버 생성 했나?");
        GameObject monster = Instantiate(monsterPrefab, transform);
        NetworkServer.Spawn(monster);

        // 모든 클라이언트에 몬스터를 스폰하라는 RPC 호출
        RpcSpawnMonster(transform.position, transform.rotation);
    }
    [ClientRpc]
    public void RpcSpawnMonster(Vector3 position, Quaternion rotation)
    {
        Debug.LogError("몬스터 생성할거야");
        Instantiate(monsterPrefab, position, rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("누구 왔다!");
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        SpawnMonster(transform);
    }
}
