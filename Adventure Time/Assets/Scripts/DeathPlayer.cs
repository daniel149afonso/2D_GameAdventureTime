using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DeathPlayer : MonoBehaviour
{
    //Player's RB
    private Rigidbody2D rb;
    //Player's Anim
    private Animator anim;
    //respawn position
    private Vector2 respawn;
    private float deathFalling = -40;
    //class PlayerMovement
    private PlayerMovement player;
    //Death Sound Effect
    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawn = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            //Save position's player at checkpoint
            respawn= transform.position;
        }
        //Collision with Trap --> Die
        if (collision.gameObject.CompareTag("Trap")|| collision.gameObject.CompareTag("Enemy"))
        {
            
            StartCoroutine(WaitBefore());
            
        }
    }
    private void Update()
    {
        CheckFalling();
    }



    private void Die()
    {
        //Play death sound
        deathSoundEffect.Play();
        //Play death animation
        anim.SetTrigger("death");
        //Freeze Movements Player
        rb.bodyType = RigidbodyType2D.Static;
        Debug.Log("Collision with a trap or an enemy");

    }
    private void CheckFalling()
    {
        if ( rb.velocity.y < deathFalling)
        {
            StartCoroutine(WaitBefore());
        }
    }
    private void CheckPoint()
    {
        anim.SetTrigger("respawn");
        transform.position = respawn;
        rb.bodyType= RigidbodyType2D.Dynamic;
    }

    private IEnumerator WaitBefore()
    {
        Die();
        yield return new WaitForSeconds(1f);
        CheckPoint();
    }
    
}

