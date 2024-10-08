using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetCamera : MonoBehaviour
{
    public CinemachineClearShot clearShot;

    public void Initialize(GameObject player)
    {
        clearShot.Follow = player.transform;
        clearShot.LookAt = player.transform;
    }
}
