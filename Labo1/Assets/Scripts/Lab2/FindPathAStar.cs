using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindPathAStar : MonoBehaviour
{
    public GameObject startPrefab, goalPrefab, pathPrefab;
    public Maze maze;

    private List<PathMarker> open = new List<PathMarker>();
    private List<PathMarker> closed = new List<PathMarker>();

    private PathMarker startNode, goalNode, lastPos;
    private bool done = false;

    void Start()
    {
        BeginSearch();
    }

    void BeginSearch()
    {
        done = false;
        open.Clear();
        closed.Clear();
        RemoveAllMarkers();

        Vector3 startPos = new Vector3(1, 0.5f, 1);
        Vector3 goalPos = new Vector3(8, 0.5f, 8);

        startNode = new PathMarker(new MapLocation(1, 1), 0, 0, 0, Instantiate(startPrefab, startPos, Quaternion.identity), null);
        goalNode = new PathMarker(new MapLocation(8, 8), 0, 0, 0, Instantiate(goalPrefab, goalPos, Quaternion.identity), null);

        open.Add(startNode);
        lastPos = startNode;

        // Automatically start the search
        while (!done && open.Count > 0)
        {
            Search(lastPos);
        }
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            Debug.Log("Goal Found!");
            StartCoroutine(FollowPath(thisNode));
            return;
        }

        List<MapLocation> neighbours = maze.GetNeighbours(thisNode.location);
        foreach (MapLocation loc in neighbours)
        {
            if (closed.Any(node => node.location.Equals(loc)))
                continue;

            float g = thisNode.G + 1;
            float h = Mathf.Abs(loc.x - goalNode.location.x) + Mathf.Abs(loc.z - goalNode.location.z);
            float f = g + h;

            if (!UpdateMarker(loc, g, h, f, thisNode))
            {
                GameObject pathMarker = Instantiate(pathPrefab, new Vector3(loc.x, 0.5f, loc.z), Quaternion.identity);
                open.Add(new PathMarker(loc, g, h, f, pathMarker, thisNode));
            }
        }
        open = open.OrderBy(p => p.F).ThenByDescending(p => p.H).ToList();
        PathMarker nextNode = open[0];
        open.RemoveAt(0);
        closed.Add(nextNode);
        lastPos = nextNode;
    }

    IEnumerator FollowPath(PathMarker node)
    {
        List<Vector3> path = new List<Vector3>();

        while (node != null)
        {
            path.Add(new Vector3(node.location.x, 0.5f, node.location.z));
            node = node.parent;
        }

        path.Reverse();

        foreach (Vector3 position in path)
        {
            transform.position = position;
            yield return new WaitForSeconds(1f);
        }
    }

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker parent)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                if (p.G > g)
                {
                    p.G = g;
                    p.H = h;
                    p.F = f;
                    p.parent = parent;
                }
                return true;
            }
        }
        return false;
    }
}