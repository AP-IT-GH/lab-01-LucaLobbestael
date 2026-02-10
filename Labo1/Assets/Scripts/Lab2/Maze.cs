using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int width = 10; //x length
    public int depth = 10; //z length
    public int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        map = new int[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                map[x, z] = Random.Range(0,3) == 0 ? 1 : 0;
                if (map[x,z] == 1)
                {
                    Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
                }
            }
        }
    }

    public List<MapLocation> GetNeighbours(MapLocation loc)
    {
        List<MapLocation> neighbours = new List<MapLocation>();
        if (loc.x > 0 && map[loc.x - 1, loc.z] == 0)neighbours.Add(new MapLocation(loc.x - 1, loc.z));
        if (loc.x < width -1 && map[loc.x+1,loc.z] == 0)neighbours.Add(new MapLocation(loc.x + 1, loc.z));
        if (loc.z > 0 && map[loc.x, loc.z -1] == 0)neighbours.Add(new MapLocation(loc.x, loc.z - 1));
        if (loc.z < depth -1 && map[loc.x, loc.z +1] == 0)neighbours.Add(new MapLocation(loc.x, loc.z + 1));
        return neighbours;
    }
}