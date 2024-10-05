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
        NetworkServer.Spawn(monster); // �������� ���� ����

        // Ŭ���̾�Ʈ������ ���� ���͸� ������ �ʿ� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        this.player = other.GetComponent<PlayerController>();
        Debug.Log("����? : " + player.userID);

        Debug.LogError("����?" + isServer);
        // ���������� SpawnMonster ȣ��
        if (isServer)
        {
            SpawnMonster(transform.position, transform.rotation);
        }
    }
}
