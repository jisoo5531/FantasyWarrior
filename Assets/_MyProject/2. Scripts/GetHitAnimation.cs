using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitAnimation : MonoBehaviour
{
    public Animator anim;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("Hit");
        }
    }
}
