using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovements : MonoBehaviour
{
    private SpriteRenderer sprite;
    //Waypoints
    [SerializeField] private GameObject[] waypoints;
    //Platform Speed
    private float speed = 3f;
    //index of the waypoints
    private int currentIndexWaypointIndex = 1;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //If distance between current point and platform position is smaller than 0
        if (Vector2.Distance(waypoints[currentIndexWaypointIndex].transform.position, transform.position) < .1f)
        {
            sprite.flipX = false;
            //change to second targeted point 
            currentIndexWaypointIndex++;
            if (currentIndexWaypointIndex >= waypoints.Length)
            {

                //change to the first targeted point
                currentIndexWaypointIndex = 0;
                sprite.flipX = true;
                
            }
        }
        //DOC: Vector2.MoveTowards(transform.position, target, step)
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentIndexWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
