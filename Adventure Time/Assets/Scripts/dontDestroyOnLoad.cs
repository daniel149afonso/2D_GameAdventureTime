using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyOnLoad : MonoBehaviour
{
    private GameObject[] music;
    private GameObject[] game;
    void Start()
    {
        music = GameObject.FindGameObjectsWithTag("gameMusic");
        Destroy(music[1]);//Evite les doublons
        
    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
