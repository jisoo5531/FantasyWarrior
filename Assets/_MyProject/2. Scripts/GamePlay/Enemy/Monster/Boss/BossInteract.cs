using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossInteract : MonoBehaviour
{
    public LayerMask playerLayer;
    public Animator bossAnim;
    public GameObject bossPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer | (1 << other.gameObject.layer)) != playerLayer) return;
        bossAnim.SetTrigger("Start");
        FindObjectOfType<CinemachineClearShot>().Follow = bossAnim.transform;
        FindObjectOfType<CinemachineClearShot>().LookAt = bossAnim.transform;

        Invoke("BossStart", 5f);
    }

    private void BossStart()
    {
        FindObjectOfType<CinemachineClearShot>().Follow = FindObjectOfType<PlayerController>().transform;
        FindObjectOfType<CinemachineClearShot>().LookAt = FindObjectOfType<PlayerController>().transform;

        Destroy(bossAnim.gameObject);
        Instantiate(bossPrefab, bossAnim.transform.position, bossAnim.transform.rotation);
        FindObjectOfType<MonsterUI>(true).gameObject.SetActive(true);

        Destroy(this.gameObject);
    }
}
