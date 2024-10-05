using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterSpawner : NetworkBehaviour
{
    public LayerMask playerLayer;
    public GameObject monsterPrefab;

    private PlayerController player;

    [Server]
    public void SpawnMonster(Vector3 position, Quaternion rotation)
    {
        GameObject monster = Instantiate(monsterPrefab, position, rotation);
        monster.GetComponent<MonsterUnit>().SetPlayer(this.player);
        monster.GetComponent<MonsterUnit>().ServerInit();
        NetworkServer.Spawn(monster); // 서버에서 몬스터 스폰

        // 클라이언트에서는 따로 몬스터를 스폰할 필요 없음
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        this.player = other.GetComponent<PlayerController>();
        Debug.Log("ㄴㄱ? : " + player.userID);

        Debug.LogError("여기?" + isServer);
        // 서버에서만 SpawnMonster 호출
        if (isServer)
        {
            SpawnMonster(transform.position, transform.rotation);
        }
    }
}
