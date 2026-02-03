using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypoint = 0;
    public float speed = 5f;
    public float rotSpeed = 2f;
    Quaternion lookRotation;

    void Start()
    {
        
    }

    void Update()
    {
        DistanceCheck();
        Rotate();
        MoveForward();
    }

    public void DistanceCheck()
    {
        float distance = Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position);
        if (distance < 10f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
        
    }

    public void Rotate()
    {
        Vector3 direction = waypoints[currentWaypoint].transform.position - transform.position;
        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime* rotSpeed);
    }
    public void MoveForward()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
