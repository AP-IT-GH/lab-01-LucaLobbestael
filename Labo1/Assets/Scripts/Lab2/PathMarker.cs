using Unity.VisualScripting;
using UnityEngine;

public class PathMarker
{
    public MapLocation location;
    public float G, H, F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        this.marker = m;
        parent = p;
    }
    public override bool Equals(object obj)
    {
        if (obj is PathMarker pm)
        {
            return location.Equals(pm.location);
        }
        return false;
    }
}
