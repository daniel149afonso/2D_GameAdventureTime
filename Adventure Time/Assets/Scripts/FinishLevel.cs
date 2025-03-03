using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Manage scene for restart player

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private AudioSource bgMusic;
    private  AudioSource finishSound;
    private Animator animFinish;
    private bool sounded = true;
    public PlayerMovement playerM;
    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
        animFinish = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            AnimCheckPoint();
            FinishSound();
            if (playerM.IsGrounded())
            {
                collision.attachedRigidbody.bodyType = RigidbodyType2D.Static;
            }

            StartCoroutine(WaitBefore());
        }
    }

    private void AnimCheckPoint()
    {
        animFinish.SetTrigger("check");
    }
    private void FinishSound()
    {
         if (sounded) {
            finishSound.Play();
         }
         sounded = false;
    }
   
    private void LoadNextLevel()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator WaitBefore()
    {
        bgMusic.Pause();
        yield return new WaitForSeconds(4f);
        finishSound.Stop();
        LoadNextLevel();
    }

    
}
