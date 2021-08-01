using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{

    public Vector3 convertGameXYtoVector3(int x, int y)
    {
        return new Vector3(x, 0, y);
    }

}
