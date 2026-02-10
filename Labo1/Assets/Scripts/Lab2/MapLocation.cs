using UnityEngine;

public struct MapLocation
{
    public int x, z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public override bool Equals(object obj)
    {
        if (obj is MapLocation ml)
        {
            return ml.x == x && ml.z == z;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return x ^ z;
    }
}
