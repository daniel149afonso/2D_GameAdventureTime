using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCheckPoint : MonoBehaviour
{
    private AudioSource audioCheck;
    private Animator anim;
    private bool sounded = true;
    private void Start()
    {
        audioCheck = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SetCheckPoint();
            SoundCheckPoint();
        }
    }

    private void SetCheckPoint()
    {
        anim.SetTrigger("check");
    }
    private void SoundCheckPoint()
    {
         if (sounded) {
            audioCheck.Play();
         }
         sounded = false;
    }
}
