using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

struct TimeSnapshot
{
    public Vector2 Pos;
    public Quaternion Rot;
    
    public TimeSnapshot (Vector2 p, Quaternion r)
    {
        Pos = p;
        Rot = r;
    }
}
