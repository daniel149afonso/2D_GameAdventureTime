using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTrampoline : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbPlayer;
    private float jumpForce = 25f;
    //Trampoline parameters
    private Animator animT;
    private AudioSource audioT;


    // Start is called before the first frame update
    void Start()
    {
        audioT = GetComponent<AudioSource>();
        animT = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            SoundTrampoline();
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, jumpForce);
            animT.SetBool("bounce", true);
            Debug.Log("You bounce");
        }
        
    }

   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            animT.SetBool("bounce", false);
        }

    }

    private void SoundTrampoline()
    {
        audioT.Play();
    }

    
        
    
}
