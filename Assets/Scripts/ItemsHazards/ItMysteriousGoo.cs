using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItMysteriousGoo : Item
{    
    public override void Use(Vector2 dropPos)
    {
        GameObject go = Instantiate(ReferenceManager.Instance.Goo, new Vector3(dropPos.x, dropPos.y, 0), new Quaternion());
        go.name = "GOO";
        base.Use(dropPos);
    }
}
