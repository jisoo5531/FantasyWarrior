using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class UI_PlayerLevelUp : MonoBehaviour
{
    public GameObject go_LevelUp;
    public TMP_Text levelText;

    private void Awake()
    {
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
        }
        EventHandler.playerEvent.RegisterPlayerLevelUp(OnLevelUP);
    }    

    private void OnLevelUP()
    {
        levelText.text = UserStatManager.Instance.userStatClient.Level.ToString();
        go_LevelUp.SetActive(true);

        Invoke("OffLevelUpUI", 2f);
    }
    private void OffLevelUpUI()
    {
        go_LevelUp.SetActive(false);
    }
}
