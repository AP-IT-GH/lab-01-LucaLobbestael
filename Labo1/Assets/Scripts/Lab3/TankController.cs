using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TankController : MonoBehaviour
{
    public WPManager wpManager;
    public TMP_Dropdown startDropdown;
    public TMP_Dropdown endDropdown;
    public float speed = 5f;
    public float rotSpeed = 2f;

    private List<Node> path;
    private int currentWaypointIndex;
    private Quaternion lookRotation;

    void Start()
    {
        PopulateDropdowns();

        startDropdown.onValueChanged.AddListener(delegate { OnDropdownChange(); });
        endDropdown.onValueChanged.AddListener(delegate { OnDropdownChange(); });
    }

    void Update()
    {
        if (path != null && currentWaypointIndex < path.Count)
        {
            DistanceCheck();
            Rotate();
            MoveForward();
        }
    }

    void PopulateDropdowns()
    {
        startDropdown.ClearOptions();
        endDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (GameObject waypoint in wpManager.waypoints)
        {
            options.Add(waypoint.name);
        }

        startDropdown.AddOptions(options);
        endDropdown.AddOptions(options);
    }

    void OnDropdownChange()
    {
        string startName = startDropdown.options[startDropdown.value].text;
        string endName = endDropdown.options[endDropdown.value].text;

        GameObject startNode = FindWaypointByName(startName);
        GameObject endNode = FindWaypointByName(endName);

        if (startNode != null && endNode != null)
        {
            if (wpManager.graph.AStar(startNode, endNode))
            {
                path = wpManager.graph.pathList;
                currentWaypointIndex = 0;
            }
        }
    }

    void DistanceCheck()
    {
        float distance = Vector3.Distance(path[currentWaypointIndex].getID().transform.position, transform.position);
        if (distance < 1f) 
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= path.Count)
            {
                path = null;
            }
        }
    }

    void Rotate()
    {
        Vector3 direction = path[currentWaypointIndex].getID().transform.position - transform.position;
        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
    }

    void MoveForward()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    GameObject FindWaypointByName(string name)
    {
        foreach (GameObject waypoint in wpManager.waypoints)
        {
            if (waypoint.name == name)
            {
                return waypoint;
            }
        }
        return null;
    }
}