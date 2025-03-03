using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Manage scene for restart player
public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            collision.rigidbody.bodyType = RigidbodyType2D.Static;
            StartCoroutine(WaitBefore());
        }
    }


    IEnumerator WaitBefore()
    {
        yield return new WaitForSeconds(3f);
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1); 
    }
}
