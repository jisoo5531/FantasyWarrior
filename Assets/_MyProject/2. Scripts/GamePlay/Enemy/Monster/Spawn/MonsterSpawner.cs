using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonsterSpawner : NetworkBehaviour
{
    public LayerMask playerLayer;
    public GameObject monsterPrefab;

    private PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        this.player = other.GetComponent<PlayerController>();
        Debug.Log("Player ID: " + player.userID);
                
        if (isServer)
        {
            SpawnMonster(transform.position, transform.rotation);
        }
    }

    [Server]
    public void SpawnMonster(Vector3 position, Quaternion rotation)
    {
        GameObject monster = Instantiate(monsterPrefab, position, rotation);
        NetworkServer.Spawn(monster);
        monster.GetComponent<MonsterUnit>().SetPlayer(this.player);
        monster.GetComponent<MonsterUnit>().ServerInit();
    }
}
