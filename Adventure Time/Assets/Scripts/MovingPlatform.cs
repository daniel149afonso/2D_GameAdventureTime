using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    //Platform Speed
    private float speed = 3f;
    //index of the waypoints
    private int currentIndexWaypointIndex = 0;
    private void Update()
    {
        //If distance between current point and platform position is smaller than 0
        if (Vector2.Distance(waypoints[currentIndexWaypointIndex].transform.position, transform.position) < .1f){
            //change to second targeted point 
            currentIndexWaypointIndex++;
            if(currentIndexWaypointIndex >= waypoints.Length)
            {
                //change to the first targeted point
                currentIndexWaypointIndex = 0;
            }
        }

        //DOC: Vector2.MoveTowards(transform.position, target, step)
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentIndexWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
