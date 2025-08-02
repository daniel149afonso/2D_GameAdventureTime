using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Manage the Canvas

public class ItemsCollecting : MonoBehaviour
{
    //Cherries count
    private int cherries = 0;

    //Display text
    [SerializeField] private Text text;

    //Sound collecting
    [SerializeField] AudioSource collectSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detect collision with fruits
        if (collision.gameObject.CompareTag("Cherry"))
        {
            //Play collecting sound
            collectSoundEffect.Play();
            //Destroy object
            Destroy(collision.gameObject);
            //Increase cherries count
            cherries++;
            //Display cherries count in text
            text.text = "Cherries: " + cherries;
            Debug.Log("Cherries: " + cherries);
        }
    }
}
